using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace InterfaceAPISwagger.Controllers
{
    public class NewUserController : ApiController
    {
        public SqlConnection con { get; private set; }

        [HttpPost]
       [Route("NewUser")]
       public JObject loginService ([FromBody] JObject loginJson)
        {
            JObject retJson = new JObject();

            try
            {
                string NAME = loginJson["NAME"].ToString();
                string USERNAME = loginJson["USERNAME"].ToString();
                string PASSWORD = loginJson["PASSWORD"].ToString();
                string EMAIL = loginJson["EMAIL"].ToString();
                string MOBILE = loginJson["MOBILE"].ToString();
                string LOGTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                con.Open();

                SqlCommand cmd = new SqlCommand("Insert into SAM_TLOGIN_USER (EMP_ID, name, username, password, email, mobile, logtime) values (@empid, @name, @usrname, @passkey, @email, @mobile, @logtime)", con);
                cmd.CommandType = CommandType.Text;
                SqlCommand cmd1 = new SqlCommand("select count(*) from SAM_TLOGIN_USER", con);
                string countUser = cmd1.ExecuteScalar().ToString();
                countUser = countUser + 1;
                cmd.Parameters.AddWithValue("@name", NAME);
                cmd.Parameters.AddWithValue("@usrname", USERNAME);
                cmd.Parameters.AddWithValue("@passkey", PASSWORD);
                cmd.Parameters.AddWithValue("@email", EMAIL);
                cmd.Parameters.AddWithValue("@mobile", MOBILE);
                cmd.Parameters.AddWithValue("@empid", "Dev_"+ countUser);
                cmd.Parameters.AddWithValue("@logtime", LOGTIME);
                cmd.ExecuteNonQuery();
                con.Close();
                retJson.Add(new JProperty("Result : ", "Welcome " + NAME + "! Your Login was created Successfully! Your Employee ID is : Dev_" + countUser));
                return retJson;
            }
            catch (Exception e)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                retJson.Add(new JProperty("Result : ", "Some error Occurred! The Error was : "+ e.Message));
                return retJson;
            }
        }
    }
}