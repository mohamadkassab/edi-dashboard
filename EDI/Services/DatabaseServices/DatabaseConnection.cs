using EDI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace EDI.Services.DatabaseServices
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EDI");
        }

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            while (true)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    return connection;
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
