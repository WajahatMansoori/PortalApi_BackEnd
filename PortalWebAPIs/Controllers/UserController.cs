using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortalWebAPIs.Models;

namespace PortalWebAPIs.Controllers
{
    public class UserController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["HrsConn"].ToString();

        [HttpGet]
        [Route("api/GetUserInfo")]
        public IHttpActionResult GetUserDetails()
        {
            List<Dictionary<string, object>> userDetailsList = new List<Dictionary<string, object>>();

            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("API_GetUserDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userDetails = new Dictionary<string, object>
                             {
                                { "UserId", reader.GetInt32(0) },
                                { "FullName", reader.GetString(1) },
                                { "DepartmentName", reader.GetString(2) }
                            };
                            userDetailsList.Add(userDetails);
                        }
                    }
                }
            }

            return Json(userDetailsList);
        }

        [HttpPost]
        [Route("api/AddUserInfo")]
        public IHttpActionResult AddUserDetail(UserDetail model)
        {
            try
            {
                List<Dictionary<string, object>> userDetailsList = new List<Dictionary<string, object>>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("API_InsertUserDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserLoginId", model.UserLoginId);
                        cmd.Parameters.AddWithValue("@FullName", model.FullName);
                        cmd.Parameters.AddWithValue("@DeptId", model.DeptId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var userDetails = new Dictionary<string, object>
                             {
                                { "Sno", reader.GetInt32(0) },
                                { "UserId", reader.GetInt32(1) },
                                { "FullName", reader.GetString(2) },
                                { "DepartmentName", reader.GetString(3) }
                            };
                                userDetailsList.Add(userDetails);
                            }
                        }
                        //cmd.Parameters.AddWithValue("@UserLoginId", model.UserLoginId);
                        //cmd.Parameters.AddWithValue("@FullName", model.FullName);
                        //cmd.Parameters.AddWithValue("@DeptId", model.DeptId);

                        //con.Open();
                        //int Rows = cmd.ExecuteNonQuery();
                        //if (Rows > 0)
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    return false;
                        //}
                    }
                    con.Close();
                }
                return Json(userDetailsList);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw ex;
            }
        }




    }
}
