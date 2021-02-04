using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace STConnect_192152K
{
    public partial class Register : System.Web.UI.Page
    {
        string DBconnect = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDB"].ConnectionString;
        static string finalpasshash;
        static string passSalt;
        byte[] ccKey;
        byte[] ccIV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            bool validated = Validation();
            // check if everything is valid
            if (validated)
            {
                string password = tb_pass.Text.ToString().Trim();

                // generate salt (random)
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                // fills byte array with strong sequence of random values
                rng.GetBytes(saltByte);
                passSalt = Convert.ToBase64String(saltByte);

                // generate hash
                SHA512Managed hashing = new SHA512Managed();

                // and salt to the password
                string passwsalt = password + passSalt;

                // hash password w salt
                byte[] hashwSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passwsalt));

                finalpasshash = Convert.ToBase64String(hashwSalt);

                // generate cypher for credit card information
                RijndaelManaged cccipher = new RijndaelManaged();
                cccipher.GenerateKey();
                ccKey = cccipher.Key;
                ccIV = cccipher.IV;


                createAccount();

                Session["registered"] = "Registered";

                Response.Redirect("Login.aspx");

            }
        }

        // validations
        protected bool Validation()
        {
            // init a bool value
            bool validate = true;

            // check if tb_fname is empty
            if (string.IsNullOrEmpty(tb_fname.Text.ToString()))
            {
                lbl_fname_status.Text = "First name cannot be empty";
                validate = false;

            }
            // check if tb_fname has only alphabets (white list)
            if(!Regex.IsMatch(tb_fname.Text.Trim(), "^[a-zA-Z]{3,20}$"))
            {
                lbl_fname_status.Text = "first name is Invalid (cannot be less than 3 characters and can only use alphabets)";
                validate = false;
            }
            // check if tb_lname is empty
            if (string.IsNullOrEmpty(tb_lname.Text.ToString()))
            {
                lbl_lname_status.Text = "Last name cannot be empty";
                validate = false;

            }
            // check if tb_fname has only alphabets (white list)
            if (!Regex.IsMatch(tb_fname.Text.Trim(), "^[a-zA-Z]{3,20}$"))
            {
                lbl_lname_status.Text = "Last name is Invalid (cannot be less than 3 characters and can only use alphabets)";
                validate = false;
            }
            // check if tb_email is empty
            if (string.IsNullOrEmpty(tb_email.Text.ToString()))
            {
                lbl_email_status.Text = "Email cannot be empty";
                validate = false;

            }
            // check if Email is valid (white list)
            if (!Regex.IsMatch(tb_email.Text.Trim(), @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{1,})$"))
            {
                lbl_email_status.Text = "Email is Invalid";
                validate = false;
            }
            //check if email exist
            if (emailExist())
            {
                lbl_email_status.Text = "Email exist";
                validate = false;
            }
            // check if tb_dob is valid
            if (!DateTime.TryParse(tb_dob.Text.ToString().Trim(), out DateTime dob))
            {
                lbl_dob_status.Text = "Invalid date of birth";
                validate = false;
            }
            // check if tb_pass contains a complex password
            if (!PassStrength())
            {
                validate = false;
            }
            // check if tb_pass is same as tb_cfmpass
            if (tb_pass.Text.ToString() != tb_cfmpass.Text.ToString())
            {
                lbl_cfmpass_status.Text = "Password and Confirm Password is not the same";
            }
            // validate credit card number
            if(!Regex.IsMatch(tb_credit_card_no.Text.ToString().Trim(), "^[0-9]{16,16}$"))
            {
                lbl_credit_status.Text = "Invalid credit card number";
            }
            // Making Credit card expiry date normal date time
            string expirydate = "28/" + tb_credit_card_expire.Text.ToString().Trim();
            // validate expiry date
            if (!DateTime.TryParse(expirydate, out DateTime expiredate))
            {
                lbl_expiry_status.Text = "Invalid expiry date";
                validate = false;
            }
            // check if expiry date pass
            if(DateTime.Compare(Convert.ToDateTime(expiredate), DateTime.Now) < 0)
            {
                lbl_expiry_status.Text = "Card expired";
                validate = false;
            }
            // validate cvv
            if  (!Regex.IsMatch(tb_credit_card_cvc.Text.ToString().Trim(), "^[0-9]{3,4}$"))
            {
                lbl_cvc_status.Text = "Invalid CVV";
                validate = false;
            }
            
            return validate;

        }

        // check pass strenght
        protected bool PassStrength()
        {
            bool strongpass = true;
            string weakness = "";

            if (tb_pass.Text.ToString().Trim().Length < 8)
            {
                weakness += "Password cannot be less than 8 character long\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_pass.Text.Trim(), "[a-z]")){
                weakness += "Password need one lower case character.\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_pass.Text.Trim(), "[A-Z]")){
                weakness += "Password need one Upper case character.\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_pass.Text.Trim(), "[0-9]")){
                weakness += "Password need one Number character.\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_pass.Text.Trim(), "[^A-Za-z0-9]")){
                weakness += "Password need one special character.";
                strongpass = false;
            }
            lbl_pass_status.Text = weakness;
            return strongpass;
        }

        public void createAccount()
        {
            try
            {
                using(SqlConnection con = new SqlConnection(DBconnect))
                {
                    using(SqlCommand cmd = new SqlCommand("INSERT INTO [Account] VALUES(@First_name, @Last_name, @Email, @DOB, @pass_salt, @passhash, @Credit_Card_Number, @Credit_Card_Expiry, @Credit_Card_CVC, 0, @CCIV, @CCKey)"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@First_name", tb_fname.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Last_name", tb_lname.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Email", tb_email.Text.ToString().Trim());
                        DateTime dob = Convert.ToDateTime(tb_dob.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@DOB", dob);
                        cmd.Parameters.AddWithValue("@pass_salt", passSalt);
                        cmd.Parameters.AddWithValue("@passhash", finalpasshash);
                        cmd.Parameters.AddWithValue("@Credit_Card_Number", Convert.ToBase64String(encryptCC(tb_credit_card_no.Text.ToString().Trim())));
                        cmd.Parameters.AddWithValue("@Credit_Card_Expiry", Convert.ToBase64String(encryptCC(tb_credit_card_expire.Text.ToString().Trim())));
                        cmd.Parameters.AddWithValue("@Credit_Card_CVC", Convert.ToBase64String(encryptCC(tb_credit_card_cvc.Text.ToString().Trim())));
                        cmd.Parameters.AddWithValue("@CCIV", Convert.ToBase64String(ccIV));
                        cmd.Parameters.AddWithValue("@CCKey", Convert.ToBase64String(ccKey));
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptCC(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged ccCipher = new RijndaelManaged();
                ccCipher.Key = ccKey;
                ccCipher.IV = ccIV;

                ICryptoTransform encryptTransformNumber = ccCipher.CreateEncryptor();
                byte[] plainTextNumber = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransformNumber.TransformFinalBlock(plainTextNumber, 0, plainTextNumber.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        protected bool emailExist()
        {
            bool exist = false;
            try
            {
                using (SqlConnection con = new SqlConnection(DBconnect)) {
                    string sqlstr = "Select Email from [Account] where Email=@email";

                    using (SqlCommand cmd = new SqlCommand(sqlstr, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@email", tb_email.Text.ToString().Trim());
                        con.Open();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                exist = true;
                            }
                            con.Close();
                            return exist;
                        }

                    }
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.ToString());
            }
        }
    }
}