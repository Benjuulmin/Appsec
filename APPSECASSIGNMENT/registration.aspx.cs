using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace APPSECASSIGNMENT
{
    public partial class registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[$&+,:;=?@#|'<>.^*()%!-]"))
            {
                score++;
            }

            return score;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        protected void createAccount()
        {
            try
            {
                int balls = FileUpload1.PostedFile.ContentLength;
                byte[] bal = new byte[balls];
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Id,@FirstName, @LastName, @CreditCard,@Email, @DoB, @Photo,@PasswordHash,@PasswordSalt,@IV,@Key)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id",Guid.NewGuid().ToString());
                            cmd.Parameters.AddWithValue("@FirstName", TextBox1.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", TextBox7.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@CreditCard", Convert.ToBase64String(encryptData(TextBox6.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", TextBox5.Text.Trim());
                            cmd.Parameters.AddWithValue("@DoB", TextBox3.Text.Trim());
                            cmd.Parameters.AddWithValue("@Photo", bal);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
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
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }




        protected void Button1_Click(object sender, EventArgs e)
        {

            int scores = checkPassword(HttpUtility.HtmlEncode(TextBox4.Text.Trim()));
            switch (scores)
            {
                case 1:
                    msg.Text = "Very Weak";
                    msg.ForeColor = Color.Red;
                    break;
                case 2:
                    msg.Text = "Weak";
                    msg.ForeColor = Color.Red;
                    break;
                case 3:
                    msg.Text = "Medium";
                    msg.ForeColor = Color.Red;
                    break;
                case 4:
                    msg.Text = "Strong";
                    msg.ForeColor = Color.Red;
                    Response.Redirect("login.aspx");
                    break;
                case 5:
                    msg.Text = "Excellent";
                    msg.ForeColor = Color.Green;
                    break;
                default:
                    break;
            }
            string pwd = TextBox4.Text.ToString().Trim(); ;
            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            createAccount();

            
        }
    }
   
}
