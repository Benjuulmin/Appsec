using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace APPSECASSIGNMENT
{
    public partial class login : System.Web.UI.Page
    {
        byte[] Key;
        byte[] IV;
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool results = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverify?secret=6LduC2IeAAAAADA8GTKMXodqrPfA3PxEdha4GfdY &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        balls.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        results = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return results;
            }
            catch (WebException ex)
            {
                throw ex;
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string userid = TextBox1.Text.ToString().Trim();
                string pwd = TextBox2.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(userid);
                string dbSalt = getDBSalt(userid);
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        Debug.WriteLine(userHash);
                        Debug.WriteLine(dbHash);
                        if (userHash.Equals(dbHash))
                        {
                            Session["LoggedIn"] = TextBox1.Text.Trim();

                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;

                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Home.aspx", false);
                        }
                        else
                        {
                            balls.Text = "wrong username or password";
                            return;
                        }
                    }
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }
            }
        }
            
            protected string getDBHash(string userid)
                {
                    string h = null;

                    SqlConnection connection = new SqlConnection(MYDBConnectionString);
                    string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@USERID", userid);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                if (reader["PasswordHash"] != null)
                                {
                                    if (reader["PasswordHash"] != DBNull.Value)
                                    {
                                        h = reader["PasswordHash"].ToString();
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally { connection.Close(); }
                    return h;
                }


                protected string getDBSalt(string userid)
                {
                    string s = null;
                    SqlConnection connection = new SqlConnection(MYDBConnectionString);
                    string sql = "select PasswordSalt FROM ACCOUNT WHERE Email=@USERID";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@USERID", userid);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["PasswordSalt"] != null)
                                {
                                    if (reader["PasswordSalt"] != DBNull.Value)
                                    {
                                        s = reader["PasswordSalt"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally { connection.Close(); }
                    return s;
                }

                protected byte[] encryptData(string data)
                {
                    byte[] cipherText = null;
                    try
                    {
                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.IV = IV;
                        cipher.Key = Key;
                        ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                        //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                        byte[] plainText = Encoding.UTF8.GetBytes(data);
                        cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
                       plainText.Length);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally { }
                    return cipherText;
                }
            }
        }
