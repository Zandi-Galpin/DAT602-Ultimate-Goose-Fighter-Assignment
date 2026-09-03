using System.Data;
using System.Data.SqlClient;

namespace GooseFighterApp
{
    public class AdminDao
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        /// <summary>
        /// Calls usp_TestAddPlayer, which inserts a new PLAYER row,
        /// the same thing the "Add new player" admin screen will do.
        /// </summary>
        public int AddPlayer(string email, string username, string password, bool isAdmin)
        {
            using (SqlConnection connection = _dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.usp_TestAddPlayer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@IsAdmin", isAdmin);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
