using System;
using System.Configuration;
using System.Data.SqlClient;

namespace GooseFighterApp
{
    /// <summary>
    /// has the connection to the GooseFighterDB database.
    /// </summary>
    public class DatabaseConnection
    {
        private static readonly string ConnectionStringValue =
            ConfigurationManager.ConnectionStrings["GooseFighterDB"].ConnectionString;

        public SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionStringValue);
        }

        /// <summary>
        /// connectivity test which will be used used by each form's "Test Connection" button.
        /// </summary>
        public bool TestConnection(out string message)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    message = "Connected successfully to " + connection.Database;
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = "Connection failed: " + ex.Message;
                return false;
            }
        }
    }
}
