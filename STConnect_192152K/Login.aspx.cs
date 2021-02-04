using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace STConnect_192152K
{
    public partial class Login : System.Web.UI.Page
    {
        string DBconnect = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["registered"] != null)
            {
                lbl_msg.Text = "Account registered Successfully";
                Session.Remove("registered");
            }        
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string pass = tb_login_pass.Text.ToString().Trim();
            string email = tb_login_email.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            int dbLoginFail = getDBLoginFail(email);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    if(!(Convert.ToInt32(dbLoginFail) < 3))
                    {
                        lbl_msg.Text = "Account locked";
                    }
                    else
                    {
                        string passWSalt = pass + dbSalt;
                        byte[] hashWSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passWSalt));
                        string userhash = Convert.ToBase64String(hashWSalt);

                        if( userhash == dbHash && ValidateCaptcha())
                        {
                            Session["loggedIn"] = tb_login_email.Text.ToString().Trim();

                            // reset failed attempts after sucessful log in
                            SqlConnection con = new SqlConnection(DBconnect);
                            string sqlstr = "UPDATE [Account] SET Login_fail = 0 WHERE Email=@email";
                            SqlCommand cmd = new SqlCommand(sqlstr, con);
                            cmd.Parameters.AddWithValue("@email", email);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            // create a GUID
                            string guid = Guid.NewGuid().ToString();
                            // save new Guid into a session
                            Session["AuthToken"] = guid;

                            // create cookie with save vaule as session "AuthToken"
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Home.aspx", false);
                        }
                        else
                        {
                            lbl_msg.Text = "Email or Password incorrect. Please try again 2";
                            addFail(email);
                        }
                    }
                }
                else
                {
                    lbl_msg.Text = "Email or Password incorrect. Please try again 1";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection con = new SqlConnection(DBconnect);
            string sqlstr = "select Password FROM [Account] WHERE Email=@Email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Password"] != null)
                        {
                            if (reader["Password"] != DBNull.Value)
                            {
                                h = reader["Password"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return h;
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection con = new SqlConnection(DBconnect);
            string sqlstr = "select Pass_salt FROM [Account] WHERE Email=@Email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Pass_salt"] != null)
                        {
                            if (reader["Pass_salt"] != DBNull.Value)
                            {
                                s = reader["Pass_salt"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return s;
        }

        protected int getDBLoginFail(string email)
        {
            int count = 0;
            SqlConnection con = new SqlConnection(DBconnect);
            string sqlstr = "select Login_fail FROM [Account] WHERE Email=@Email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Login_fail"] != null)
                        {
                            if (reader["Login_fail"] != DBNull.Value)
                            {
                                string strcount = reader["Login_fail"].ToString();
                                count = int.Parse(strcount);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return count;
        }

        protected void addFail(string email)
        {
            int Fails = getDBLoginFail(email);

            SqlConnection con = new SqlConnection(DBconnect);
            string sqlstr = "UPDATE [Account] SET Login_fail =@login_fail WHERE Email=@email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            int addfail = Fails + 1;
            cmd.Parameters.AddWithValue("@login_fail", addfail);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public bool ValidateCaptcha()
        {
            bool result = true;

            
            string captchaResponse = Request.Form["g-recaptcha-response"];

            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverify?secret=6Lezv0kaAAAAAE2SXbd_gCiW8HINEhnBKoWz8M76 &response=" + captchaResponse);

            try
            {
                
                using (WebResponse webRes = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(webRes.GetResponseStream()))
                    {
                        // The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        // Create jsonObject to handle the response i.e. success or error
                        // Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        // Convert the string
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }


    }
}