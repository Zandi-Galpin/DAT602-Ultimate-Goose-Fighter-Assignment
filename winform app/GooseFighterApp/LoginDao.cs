using System.Data;
using System.Data.SqlClient;

namespace GooseFighterApp
{
    public class LoginDao
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        /// <summary>
        /// Calls usp_TestLogin, which checks a username and password against
        /// the Player table. Returns a datatable with PlayerID, IsAdmin,and
        /// IsLockedOut for the matching row ( or empty if there isnt a match).
        /// </summary>
        public DataTable AttemptLogin(string username, string password)
        {
            DataTable result = new DataTable();

            using (SqlConnection connection = _dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.usp_TestLogin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }
    }
}
