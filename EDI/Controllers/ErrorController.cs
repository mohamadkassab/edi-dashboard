using EDI.Services.DatabaseServices;
using EDI.Utilities.ItemDataUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EDI.Controllers
{
    [Authorize(Roles = "IT_SoftwareDeveloper")]
    public class ErrorController : Controller
    {
        private readonly DatabaseConnection _connection;
        private readonly IpAddress _ipAddress;
 
        public ErrorController(IConfiguration configuration)
        {
            _connection = new DatabaseConnection(configuration);
            _ipAddress = new IpAddress();
        }
        
        [HttpPost]
        public async void Insert(string className, string functionName, string errorMessage, string stackTrace)
        {         
            try
            {
                using (SqlConnection connection = await _connection.OpenConnectionAsync())
                {
                    string ip = _ipAddress.Get();
                    string userID = HttpContext?.Session?.GetString("UserEmail");

                    using (SqlCommand command = new SqlCommand("EDIA_ErrorLogs_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IpAddress", ip);
                        command.Parameters.AddWithValue("@UserId", userID);
                        command.Parameters.AddWithValue("@ClassName", className);
                        command.Parameters.AddWithValue("@FunctionName", functionName);
                        command.Parameters.AddWithValue("@ErrorMessage", errorMessage);
                        command.Parameters.AddWithValue("@StackTrace", stackTrace);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
