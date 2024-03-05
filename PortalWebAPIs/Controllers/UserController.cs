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


namespace PortalWebAPIs.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/GetUserInfo")]
        public IHttpActionResult GetUserDetails()
        {
            List<Dictionary<string, object>> userDetailsList = new List<Dictionary<string, object>>();

            string connectionString = ConfigurationManager.ConnectionStrings["HrsConn"].ToString();

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



    }
}
