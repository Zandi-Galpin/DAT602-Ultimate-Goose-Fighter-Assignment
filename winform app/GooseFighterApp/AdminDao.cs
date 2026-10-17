using System;
using System.Data;
using System.Data.SqlClient;

namespace GooseFighterApp
{
    public class AdminDao : BaseDao
    {
        /// <summary>
        /// crud 3. Calls usp_PlaceTile. Result contains TileID, PlaceTileStatus
        /// (which is SUCCESS, TILE_EXISTS or NOT_ADJACENT).
        /// </summary>
        public DataTable PlaceTile(int x, int y)
        {
            return ExecuteProcedure("dbo.usp_PlaceTile",
                Param("@XPosition", SqlDbType.Int, x),
                Param("@YPosition", SqlDbType.Int, y));
        }

        /// <summary>
        /// CRUD 3. Calls usp_DeleteTile. Result contains: DeleteTileStatus
        /// (which is SUCCESS, TILE_NOT_FOUND or TILE_IN_USE).
        /// </summary>
        public DataTable DeleteTile(int tileId)
        {
            return ExecuteProcedure("dbo.usp_DeleteTile",
                Param("@TileID", SqlDbType.Int, tileId));
        }

        /// <summary>
        /// crud 4 calls usp_PlaceItem. Result containss ItemID, PlaceItemStatus
        /// (which is SUCCESS, ITEMTYPE_NOT_FOUND, TILE_NOT_FOUND or TILE_HAS_ITEM).
        /// </summary>
        public DataTable PlaceItem(int itemTypeId, int tileId)
        {
            return ExecuteProcedure("dbo.usp_PlaceItem",
                Param("@ItemTypeID", SqlDbType.Int, itemTypeId),
                Param("@TileID", SqlDbType.Int, tileId));
        }

        /// <summary>
        /// crud 4. Calls usp_RemoveItemFromTile (rubbish bin tool).
        /// Result contains RemoveItemStatus (SUCCESS or NO_ITEM).
        /// </summary>
        public DataTable RemoveItemFromTile(int tileId)
        {
            return ExecuteProcedure("dbo.usp_RemoveItemFromTile",
                Param("@TileID", SqlDbType.Int, tileId));
        }

        /// <summary>
        /// CRUD 9 calls usp_KillRunningGame. Result contains PlayersLoggedOut, KillStatus
        /// (which is SUCCESS, ADMIN_NOT_FOUND or NOT_ADMIN).
        /// </summary>
        public DataTable KillRunningGame(int adminId)
        {
            return ExecuteProcedure("dbo.usp_KillRunningGame",
                Param("@AdminID", SqlDbType.Int, adminId));
        }

        /// <summary>
        /// CRUD 10 calls usp_AddPlayer. Result has PlayerID, AddPlayerStatus
        /// (has SUCCESS, ADMIN_NOT_FOUND, NOT_ADMIN, MISSING_FIELDS, USERNAME_TAKEN or EMAIL_TAKEN).
        /// </summary>
        public DataTable AddPlayer(int adminId, string email, string username, string password, bool isAdmin)
        {
            return ExecuteProcedure("dbo.usp_AddPlayer",
                Param("@AdminID", SqlDbType.Int, adminId),
                Param("@Email", SqlDbType.VarChar, email),
                Param("@Username", SqlDbType.VarChar, username),
                Param("@Password", SqlDbType.VarChar, password),
                Param("@IsAdmin", SqlDbType.Bit, isAdmin));
        }

        /// <summary>
        /// CRUD 11 calls usp_UpdatePlayer. Result has PlayerID, UpdatePlayerStatus
        /// (has SUCCESS, ADMIN_NOT_FOUND, NOT_ADMIN, PLAYER_NOT_FOUND, MISSING_FIELDS,
        /// INVALID_SCORE, USERNAME_TAKEN or EMAIL_TAKEN).
        /// </summary>
        public DataTable UpdatePlayer(int adminId, int playerId, string email, string username,
                                      string password, int score, bool isAdmin, bool isLockedOut)
        {
            return ExecuteProcedure("dbo.usp_UpdatePlayer",
                Param("@AdminID", SqlDbType.Int, adminId),
                Param("@PlayerID", SqlDbType.Int, playerId),
                Param("@Email", SqlDbType.VarChar, email),
                Param("@Username", SqlDbType.VarChar, username),
                Param("@Password", SqlDbType.VarChar, password),
                Param("@Score", SqlDbType.Int, score),
                Param("@IsAdmin", SqlDbType.Bit, isAdmin),
                Param("@IsLockedOut", SqlDbType.Bit, isLockedOut));
        }

        /// <summary>
        /// CRUD 12 calls usp_DeletePlayer. A player deleting their account passes
        /// their ID as both arguments. Result has DeletePlayerStatus (has SUCCESS,
        /// REQUESTER_NOT_FOUND, PLAYER_NOT_FOUND or NOT_AUTHORISED).
        /// </summary>
        public DataTable DeletePlayer(int requesterId, int playerId)
        {
            return ExecuteProcedure("dbo.usp_DeletePlayer",
                Param("@RequesterID", SqlDbType.Int, requesterId),
                Param("@PlayerID", SqlDbType.Int, playerId));
        }

        /// <summary>
        /// Calls usp_GetItemTypes. Rows: ItemTypeID, Name, Category, EffectDescription, SpawnRate.
        /// Fills the item picker on the admin screen.
        /// </summary>
        public DataTable GetItemTypes()
        {
            return ExecuteProcedure("dbo.usp_GetItemTypes");
        }

        /// <summary>
        /// Calls usp_GetAllPlayers. One row per player:
        /// PlayerID, Username, Email, Score, IsAdmin, IsOnline, IsLockedOut (no passwords).
        /// </summary>
        public DataTable GetAllPlayers()
        {
            return ExecuteProcedure("dbo.usp_GetAllPlayers");
        }

        /// <summary>
        /// Calls usp_GetPlayer. One row (no row if the player doesn't exist):
        /// PlayerID, Email, Username, Password, Score, Health, IsAdmin, IsOnline, IsLockedOut,
        /// CurrentTileID, X, Y, HeldItem, HeldAmmo. The admin edit boxes show the password
        /// in plaintext (storyboard 11).
        /// </summary>
        public DataTable GetPlayer(int playerId)
        {
            return ExecuteProcedure("dbo.usp_GetPlayer",
                Param("@PlayerID", SqlDbType.Int, playerId));
        }

        /// <summary>
        /// Calls usp_GetBoard. One row per tile: TileID, X, Y, Goose, Npc, Item, NestOf.
        /// </summary>
        public DataTable GetBoard()
        {
            return ExecuteProcedure("dbo.usp_GetBoard");
        }

        /// <summary>
        /// old method (usp_TestAddPlayer, no admin check).
        /// AdminForm still calls this until i switched
        /// </summary>
        [Obsolete("Replaced by AddPlayer(adminId, ...). Delete once AdminForm is updated.")]
        public int AddPlayer(string email, string username, string password, bool isAdmin)
        {
            using (SqlConnection connection = _dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.usp_TestAddPlayer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Param("@Email", SqlDbType.VarChar, email));
                command.Parameters.Add(Param("@Username", SqlDbType.VarChar, username));
                command.Parameters.Add(Param("@Password", SqlDbType.VarChar, password));
                command.Parameters.Add(Param("@IsAdmin", SqlDbType.Bit, isAdmin));

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}