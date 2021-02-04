using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace STConnect_192152K
{
    public partial class Home : System.Web.UI.Page
    {
        string DBconnect = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDB"].ConnectionString;
        string fname;
        string lname;
        string email;
        string dob;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"].Value != null)
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    lbl_login_status.Text = "Logged in";
                    lbl_login_status.Visible = true;
                    lbl_welcome.Visible = true;
                    lbl_email.Visible = true;
                    lbl_DOB.Visible = true;
                    btn_logout.Visible = true;

                    SqlConnection con = new SqlConnection(DBconnect);
                    string sqlstr = "select * from [Account] where Email = @email";
                    SqlCommand cmd = new SqlCommand(sqlstr, con);
                    cmd.Parameters.AddWithValue("@email", Session["loggedIn"].ToString());

                    try
                    {
                        con.Open();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if(reader["Email"] != DBNull.Value)
                                {
                                    email = reader["Email"].ToString();
                                }
                                if(reader["dob"] != DBNull.Value)
                                {
                                    dob = reader["dob"].ToString();
                                }
                                if(reader["First_name"] != DBNull.Value)
                                {
                                    fname = reader["First_name"].ToString();
                                }
                                if(reader["Last_name"] != DBNull.Value)
                                {
                                    lname = reader["Last_name"].ToString();
                                }
                            }
                        }
                    } 
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                    lbl_DOB.Text = "DOB: " + dob;
                    lbl_email.Text = "Email: " + email;
                    lbl_welcome.Text = "Welcome " + fname + lname;


                }
                else
                {
                    lbl_login_status.Text = "Not Logged in";
                    lbl_login_status.Visible = true;
                    btn_Login.Visible = true;
                    hl_register.Visible = true;
                }
            }
            else
            {
                lbl_login_status.Text = "Not Logged in";
                lbl_login_status.Visible = true;
                btn_Login.Visible = true;
                hl_register.Visible = true;
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}