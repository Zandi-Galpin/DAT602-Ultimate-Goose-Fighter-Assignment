using System.Data;
using System.Data.SqlClient;

namespace GooseFighterApp
{
    public class GamePlayDao
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        /// <summary>
        /// Calls usp_TestMovePlayer, which updates a player's CurrentTileID.
        /// This is temporary to show it works
        /// </summary>
        public int MovePlayer(int playerId, int newTileId)
        {
            using (SqlConnection connection = _dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.usp_TestMovePlayer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PlayerID", playerId);
                command.Parameters.AddWithValue("@NewTileID", newTileId);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
