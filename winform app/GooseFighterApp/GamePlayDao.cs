using System;
using System.Data;
using System.Data.SqlClient;

namespace GooseFighterApp
{
    public class GamePlayDao : BaseDao
    {
        /// <summary>
        /// CRUD 5. Calls usp_MovePlayer. direction is UP, DOWN, LEFT or RIGHT.
        /// Result contain NewTileID, NewX, NewY, MoveStatus (SUCCESS, OFF_BOARD,
        /// TILE_OCCUPIED, INVALID_DIRECTION, PLAYER_NOT_FOUND, NOT_ONLINE, NO_CURRENT_TILE).
        /// </summary>
        public DataTable MovePlayer(int playerId, string direction)
        {
            return ExecuteProcedure("dbo.usp_MovePlayer",
                Param("@PlayerID", SqlDbType.Int, playerId),
                Param("@Direction", SqlDbType.VarChar, direction));
        }

        /// <summary>
        /// CRUD 6. Calls usp_Attack. Result contains AttackStatus, DamageDealt,
        /// VictimPlayerID, VictimHealth, AttackerScore. AttackStatus is HIT, KILLED,
        /// NPC_HIT, NPC_KILLED, TARGET_INVINCIBLE, TARGET_ON_NEST, NOTHING_THERE,
        /// INVALID_DIRECTION, ATTACKER_NOT_FOUND, ATTACKER_IS_ADMIN, NOT_ONLINE or NO_CURRENT_TILE.
        /// </summary>
        public DataTable Attack(int attackerId, string direction)
        {
            return ExecuteProcedure("dbo.usp_Attack",
                Param("@AttackerID", SqlDbType.Int, attackerId),
                Param("@Direction", SqlDbType.VarChar, direction));
        }

        /// <summary>
        /// CRUD 7. Calls usp_PickUpItem (the item on the player's own tile).
        /// Result contains PickUpStatus, ItemID, ItemName, Ammo.
        /// </summary>
        public DataTable PickUpItem(int playerId)
        {
            return ExecuteProcedure("dbo.usp_PickUpItem",
                Param("@PlayerID", SqlDbType.Int, playerId));
        }

        /// <summary>
        /// CRUD 7. Calls usp_DropItem (stores in the nest if standing on it, otherwise
        /// the item disappears). Result contains DropStatus, ItemID.
        /// </summary>
        public DataTable DropItem(int playerId)
        {
            return ExecuteProcedure("dbo.usp_DropItem",
                Param("@PlayerID", SqlDbType.Int, playerId));
        }

        /// <summary>
        /// CRUD 7. Calls usp_TakeFromNest. Result contains TakeStatus, ItemID.
        /// </summary>
        public DataTable TakeFromNest(int playerId, int itemId)
        {
            return ExecuteProcedure("dbo.usp_TakeFromNest",
                Param("@PlayerID", SqlDbType.Int, playerId),
                Param("@ItemID", SqlDbType.Int, itemId));
        }

        /// <summary>
        /// CRUD 4 (random spawning). Calls usp_SpawnRandomItem.
        /// Result contains ItemID, TileID, ItemTypeID, SpawnStatus.
        /// </summary>
        public DataTable SpawnRandomItem()
        {
            return ExecuteProcedure("dbo.usp_SpawnRandomItem");
        }

        /// <summary>
        /// CRUD 8. Calls usp_MoveNpc (one random step). Result contains
        /// NewTileID, NewX, NewY, MoveNpcStatus.
        /// </summary>
        public DataTable MoveNpc(int npcId)
        {
            return ExecuteProcedure("dbo.usp_MoveNpc",
                Param("@NpcID", SqlDbType.Int, npcId));
        }

        /// <summary>
        /// CRUD 8. Calls usp_RespawnNpc. Result contains NpcID, TileID, RespawnStatus.
        /// </summary>
        public DataTable RespawnNpc()
        {
            return ExecuteProcedure("dbo.usp_RespawnNpc");
        }

        /// <summary>
        /// CRUD 8. Calls usp_DamageNpc, which returns its results
        /// Returns true if this hit killed the NPC.
        /// npcHealthLeft is null when the NPC doesn't exist or was already dead.
        /// </summary>
        public bool DamageNpc(int npcId, int damage, out int? npcHealthLeft)
        {
            SqlParameter healthLeft = new SqlParameter("@NpcHealthLeft", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            SqlParameter killed = new SqlParameter("@NpcKilled", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            using (SqlConnection connection = _dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.usp_DamageNpc", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Param("@NpcID", SqlDbType.Int, npcId));
                command.Parameters.Add(Param("@Damage", SqlDbType.Int, damage));
                command.Parameters.Add(healthLeft);
                command.Parameters.Add(killed);

                connection.Open();
                command.ExecuteNonQuery();
            }

            npcHealthLeft = healthLeft.Value == DBNull.Value ? (int?)null : (int)healthLeft.Value;
            return killed.Value != DBNull.Value && (bool)killed.Value;
        }

        /// <summary>
        /// old prototype method (usp_TestMovePlayer).
        /// GamePlayForm still calls this until it is switched later
        /// </summary>
        [Obsolete("Replaced by MovePlayer(playerId, direction). Delete once GamePlayForm is updated.")]
        public int MovePlayer(int playerId, int newTileId)
        {
            using (SqlConnection connection = _dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.usp_TestMovePlayer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Param("@PlayerID", SqlDbType.Int, playerId));
                command.Parameters.Add(Param("@NewTileID", SqlDbType.Int, newTileId));

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}