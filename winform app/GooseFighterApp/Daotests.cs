using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace GooseFighterApp
{
    /// <summary>
    /// Tests the DAO methods
    /// Each test group rebuilds database using the database setup script
    /// script so that every group starts with the same test data.
    /// The tests call the DAO methods and check the results.
    ///
    /// Only run this with test data.
    /// Sql() calls are used to set up test scenarios.
    /// All the methods tested are called through the DAO classes.
    /// </summary>
    public class DaoTests
    {
        private readonly LoginDao _login = new LoginDao();
        private readonly GamePlayDao _game = new GamePlayDao();
        private readonly AdminDao _admin = new AdminDao();
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private readonly StringBuilder _log = new StringBuilder();
        private int _passed;
        private int _failed;

        public string RunAll()
        {
            _log.Clear();
            _passed = 0;
            _failed = 0;

            RunGroup("CRUD 1 Player login and lockout", LoginTests);
            RunGroup("CRUD 2 Player registration", RegisterTests);
            RunGroup("CRUD 3 Laying out tiles", TileTests);
            RunGroup("CRUD 4 Placing items on tiles", PlaceItemTests);
            RunGroup("CRUD 4 Random item spawning", SpawnTests);
            RunGroup("CRUD 5 Player movement", MoveTests);
            RunGroup("CRUD 6 Attacking and scoring", AttackTests);
            RunGroup("CRUD 7 Acquiring inventory", InventoryTests);
            RunGroup("CRUD 8 NPC movement, damage and respawn", NpcTests);
            RunGroup("CRUD 9 Ending a game and logging out", KillGameTests);
            RunGroup("CRUD 10 Adding new players", AddPlayerTests);
            RunGroup("CRUD 11 Updating players", UpdatePlayerTests);
            RunGroup("CRUD 12 Deleting players", DeletePlayerTests);

            _log.AppendLine();
            _log.AppendLine("TOTAL: " + _passed + " passed, " + _failed + " failed");
            return _log.ToString();
        }

        // Login and registration tests

        private void LoginTests()
        {
            Expect("correct password", () => _login.Login("ZandiGoose", "Password123"), "LoginStatus", "SUCCESS");
            Expect("wrong password", () => _login.Login("ZandiGoose", "wrongpassXD"), "LoginStatus", "INCORRECT_PASSWORD");
            Expect("account is already locked", () => _login.Login("LockedGoose", "wrongpass"), "LoginStatus", "LOCKED_OUT");
            Expect("username doesnt exist", () => _login.Login("NotArealPlayer", "skibidi"), "LoginStatus", "NOT_FOUND");

            // Five wrong attempts should lock McPlayerTwoington's account.
            // After that, the correct password should be rejected.
            for (int attempt = 1; attempt <= 4; attempt++)
            {
                Expect("McPlayer wrong password, attempt " + attempt,
                       () => _login.Login("McPlayerTwoington", "nope"), "LoginStatus", "INCORRECT_PASSWORD");
            }
            Expect("McPlayer gets locked out after five wrong attempts",
                   () => _login.Login("McPlayerTwoington", "nope"), "LoginStatus", "LOCKED_OUT");
            Expect("correct password is rejected after lockout",
                   () => _login.Login("McPlayerTwoington", "Passw0rd!"), "LoginStatus", "LOCKED_OUT");
            ExpectValue("account is marked as locked in the database",
                        () => Scalar("SELECT IsLockedOut FROM dbo.Player WHERE PlayerID = 3"), true);
        }

        private void RegisterTests()
        {
            DataTable created = Expect("register a new player", () => _login.Register("newguyforreal@example.com", "NewPlayer1", "Passwooord!1"), "RegisterStatus", "SUCCESS");
            ExpectValue("new player gets PlayerID 6", () => created.Rows[0]["PlayerID"], 6);
            ExpectValue("new player starts with a score of 0", () => Scalar("SELECT Score FROM dbo.Player WHERE Username = 'NewPlayer1'"), 0);
            Expect("username is already taken", () => _login.Register("another@example.com", "ZandiGoose", "Passw0rd!2"), "RegisterStatus", "USERNAME_TAKEN");
            Expect("email is already taken", () => _login.Register("zandi@example.com", "SkibidiPlayer", "Password1!"), "RegisterStatus", "EMAIL_TAKEN");
            Expect("email is missing", () => _login.Register("", "NoEmailGoose", "Passw0rd!"), "RegisterStatus", "MISSING_FIELDS");
        }

        // Tile and item tests

        private void TileTests()
        {
            DataTable placed = Expect("place a tile next to an existing one", () => _admin.PlaceTile(5, 2), "PlaceTileStatus", "SUCCESS");
            Expect("place a tile on coordinates that are already used", () => _admin.PlaceTile(0, 0), "PlaceTileStatus", "TILE_EXISTS");
            Expect("place a tile that isnt next to another tile", () => _admin.PlaceTile(9, 9), "PlaceTileStatus", "NOT_ADJACENT");
            Expect("delete a tile with a player and nest on it", () => _admin.DeleteTile(13), "DeleteTileStatus", "TILE_IN_USE");
            Expect("delete a tile that has an item on it", () => _admin.DeleteTile(19), "DeleteTileStatus", "TILE_IN_USE");
            Expect("delete a tile that doesnt exist", () => _admin.DeleteTile(999), "DeleteTileStatus", "TILE_NOT_FOUND");

            // SCOPE_IDENTITY() returns a decimal, convert it to an int.
            int newTileId = placed != null ? Convert.ToInt32(placed.Rows[0]["TileID"]) : -1;
            Expect("delete the empty tile we just placed", () => _admin.DeleteTile(newTileId), "DeleteTileStatus", "SUCCESS");
        }

        private void PlaceItemTests()
        {
            Expect("place a banana on empty tile 20", () => _admin.PlaceItem(6, 20), "PlaceItemStatus", "SUCCESS");
            Expect("place a pellet gun on empty tile 21", () => _admin.PlaceItem(2, 21), "PlaceItemStatus", "SUCCESS");
            ExpectValue("pellet gun ammo is between 3 and 10", () =>
            {
                int ammo = Convert.ToInt32(Scalar("SELECT Ammo FROM dbo.Item WHERE TileID = 21"));
                return ammo >= 3 && ammo <= 10;
            }, true);
            Expect("place an item on a tile that already has one", () => _admin.PlaceItem(6, 19), "PlaceItemStatus", "TILE_HAS_ITEM");
            Expect("use an item type that doesnt exist", () => _admin.PlaceItem(99, 22), "PlaceItemStatus", "ITEMTYPE_NOT_FOUND");
            Expect("place an item on a tile that doesnt exist", () => _admin.PlaceItem(6, 999), "PlaceItemStatus", "TILE_NOT_FOUND");
            Expect("remove the banana using the rubbish bin", () => _admin.RemoveItemFromTile(20), "RemoveItemStatus", "SUCCESS");
            Expect("remove an item from an empty tile", () => _admin.RemoveItemFromTile(20), "RemoveItemStatus", "NO_ITEM");
        }

        private void SpawnTests()
        {
            // Only ZandiGoose is online at the start, and two items are already on tiles.
            Expect("spawning with only one player online", () => _game.SpawnRandomItem(), "SpawnStatus", "NOT_ENOUGH_PLAYERS");
            Sql("UPDATE dbo.Player SET IsOnline = 1 WHERE PlayerID = 3;");
            Expect("spawn an item with two players online", () => _game.SpawnRandomItem(), "SpawnStatus", "SUCCESS");
            Expect("spawn a second item", () => _game.SpawnRandomItem(), "SpawnStatus", "SUCCESS");
            Expect("stop spawning when the item limit is reached", () => _game.SpawnRandomItem(), "SpawnStatus", "ITEM_LIMIT_REACHED");
        }

        // Player movement tests

        private void MoveTests()
        {
            // ZandiGoose (PlayerID 2) starts online on tile 13 at coordinates (2,2).
            DataTable first = Expect("move up onto an empty tile", () => _game.MovePlayer(2, "UP"), "MoveStatus", "SUCCESS");
            ExpectValue("player moves to tile 12", () => first.Rows[0]["NewTileID"], 12);
            Expect("move up into the NPC", () => _game.MovePlayer(2, "UP"), "MoveStatus", "TILE_OCCUPIED");
            Expect("move left onto a tile with an offline player", () => _game.MovePlayer(2, "LEFT"), "MoveStatus", "SUCCESS");

            Sql("UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 8 WHERE PlayerID = 3;");
            Expect("move down into an online player", () => _game.MovePlayer(2, "DOWN"), "MoveStatus", "TILE_OCCUPIED");
            Expect("move up onto a tile with an item", () => _game.MovePlayer(2, "UP"), "MoveStatus", "SUCCESS");
            Expect("move left to the edge of the board", () => _game.MovePlayer(2, "LEFT"), "MoveStatus", "SUCCESS");
            Expect("move left off the board", () => _game.MovePlayer(2, "LEFT"), "MoveStatus", "OFF_BOARD");
            Expect("use an invalid direction", () => _game.MovePlayer(2, "SKIBIDI"), "MoveStatus", "INVALID_DIRECTION");
            Expect("move a player that doesnt exist", () => _game.MovePlayer(991, "UP"), "MoveStatus", "PLAYER_NOT_FOUND");
            Expect("move a player who is offline", () => _game.MovePlayer(5, "UP"), "MoveStatus", "NOT_ONLINE");
            Sql("UPDATE dbo.Player SET IsOnline = 1 WHERE PlayerID = 5;");
            Expect("move an online player who has no current tile", () => _game.MovePlayer(5, "UP"), "MoveStatus", "NO_CURRENT_TILE");
        }

        // Attacking and scoring tests

        private void AttackTests()
        {
            // Put ZandiGoose (2) on tile 13 with the bat and McPlayer (3) on tile 14.
            Sql("UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 13, Health = 4000, IsInvincible = 0, InvincibleUntil = NULL WHERE PlayerID = 2;" +
                "UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 14, Health = 4000, IsInvincible = 0, InvincibleUntil = NULL WHERE PlayerID = 3;");

            DataTable hit = Expect("bat attack hits McPlayer", () => _game.Attack(2, "DOWN"), "AttackStatus", "HIT");
            ExpectValue("bat deals 60 damage", () => hit.Rows[0]["DamageDealt"], 60);
            ExpectValue("McPlayer has 3940 health left", () => hit.Rows[0]["VictimHealth"], 3940);
            Expect("attack again while the target is invincible", () => _game.Attack(2, "DOWN"), "AttackStatus", "TARGET_INVINCIBLE");
            Thread.Sleep(1000);
            DataTable hit2 = Expect("attack again after invincibility ends", () => _game.Attack(2, "DOWN"), "AttackStatus", "HIT");
            ExpectValue("McPlayer has 3880 health left", () => hit2.Rows[0]["VictimHealth"], 3880);
            Expect("attack a player standing on their nest", () => _game.Attack(3, "UP"), "AttackStatus", "TARGET_ON_NEST");

            Sql("UPDATE dbo.Player SET Health = 50, IsInvincible = 0, InvincibleUntil = NULL WHERE PlayerID = 3;");
            DataTable kill = Expect("land a lethal hit", () => _game.Attack(2, "DOWN"), "AttackStatus", "KILLED");
            ExpectValue("attacker's score increases from 3 to 4", () => kill.Rows[0]["AttackerScore"], 4);
            ExpectValue("victim respawns with full health", () => Scalar("SELECT Health FROM dbo.Player WHERE PlayerID = 3"), 4000);
            ExpectValue("victim has one nest after respawning", () => Scalar("SELECT COUNT(*) FROM dbo.Nest WHERE PlayerID = 3"), 1);
            ExpectValue("victim's old nest on tile 7 is removed", () => Scalar("SELECT COUNT(*) FROM dbo.Nest WHERE PlayerID = 3 AND TileID = 7"), 0);

            Sql("UPDATE dbo.Player SET CurrentTileID = 12 WHERE PlayerID = 2;");
            DataTable npcHit = Expect("attack the NPC", () => _game.Attack(2, "UP"), "AttackStatus", "NPC_HIT");
            ExpectValue("NPC has 940 health left", () => npcHit.Rows[0]["VictimHealth"], 940);
            ExpectValue("attacking the NPC doesnt increase the score", () => Scalar("SELECT Score FROM dbo.Player WHERE PlayerID = 2"), 4);

            Sql("UPDATE dbo.Player SET CurrentTileID = 1 WHERE PlayerID = 2;");
            Expect("attack towards the edge of the board", () => _game.Attack(2, "LEFT"), "AttackStatus", "NOTHING_THERE");
            Expect("use an invalid attack direction", () => _game.Attack(2, "SIDEWAYS"), "AttackStatus", "INVALID_DIRECTION");
            Expect("attack with a player that doesn't exist", () => _game.Attack(999, "UP"), "AttackStatus", "ATTACKER_NOT_FOUND");
            Expect("admin tries to attack", () => _game.Attack(1, "UP"), "AttackStatus", "ATTACKER_IS_ADMIN");
            Expect("offline player tries to attack", () => _game.Attack(5, "UP"), "AttackStatus", "NOT_ONLINE");
            Sql("UPDATE dbo.Player SET IsOnline = 1 WHERE PlayerID = 5;");
            Expect("online attacker has no current tile", () => _game.Attack(5, "UP"), "AttackStatus", "NO_CURRENT_TILE");
        }

        // inventory and nest tests

        private void InventoryTests()
        {
            // ZandiGoose (2) has the bat. His nest is on tile 13 and has a shield and peas.
            Sql("UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 19 WHERE PlayerID = 2;");
            Expect("try to pick up an item while already holding the bat", () => _game.PickUpItem(2), "PickUpStatus", "INVENTORY_FULL");

            Sql("UPDATE dbo.Player SET CurrentTileID = 13 WHERE PlayerID = 2;");
            Expect("store the bat by dropping it on the player's nest", () => _game.DropItem(2), "DropStatus", "STORED_IN_NEST");
            Expect("drop an item when holding nothing", () => _game.DropItem(2), "DropStatus", "NOTHING_HELD");

            Sql("UPDATE dbo.Player SET CurrentTileID = 19 WHERE PlayerID = 2;");
            DataTable pick = Expect("pick up the watermelon", () => _game.PickUpItem(2), "PickUpStatus", "SUCCESS");
            ExpectValue("picked-up item is a watermelon", () => pick.Rows[0]["ItemName"], "Watermelon");
            Expect("pick up an item from an empty tile", () => _game.PickUpItem(2), "PickUpStatus", "NO_ITEM");

            Sql("UPDATE dbo.Player SET CurrentTileID = 12 WHERE PlayerID = 2;");
            Expect("drop an item away from the nest", () => _game.DropItem(2), "DropStatus", "DISCARDED");
            Expect("take an item from a nest while standing elsewhere", () => _game.TakeFromNest(2, 2), "TakeStatus", "NOT_ON_NEST");

            Sql("UPDATE dbo.Player SET CurrentTileID = 13 WHERE PlayerID = 2;");
            Expect("take an item that isnt in the nest", () => _game.TakeFromNest(2, 999), "TakeStatus", "ITEM_NOT_IN_NEST");
            Expect("take the shield from the nest", () => _game.TakeFromNest(2, 2), "TakeStatus", "SUCCESS");
            Expect("try to take peas while holding the shield", () => _game.TakeFromNest(2, 3), "TakeStatus", "INVENTORY_FULL");

            // Add seven more items to the nest so it reaches the item limit.
            Sql("INSERT INTO dbo.Item (ItemTypeID, NestID) VALUES (5, 1),(5, 1),(5, 1),(5, 1),(5, 1),(5, 1),(5, 1);");
            Expect("try to store an item in a full nest", () => _game.DropItem(2), "DropStatus", "NEST_FULL");

            // Set up two pellet guns to check that their ammo gets added together.
            // The player has four rounds and the gun on tile 6 has seven.
            Sql("DELETE FROM dbo.Item WHERE HolderPlayerID = 2;" +
                "INSERT INTO dbo.Item (ItemTypeID, HolderPlayerID, Ammo) VALUES (2, 2, 4);" +
                "UPDATE dbo.Player SET CurrentTileID = 6 WHERE PlayerID = 2;");
            DataTable stacked = Expect("pick up a second pellet gun", () => _game.PickUpItem(2), "PickUpStatus", "AMMO_ADDED");
            ExpectValue("ammo adds up to 11", () => stacked.Rows[0]["Ammo"], 11);

            Expect("pick up an item for a player that doesnt exist", () => _game.PickUpItem(999), "PickUpStatus", "PLAYER_NOT_FOUND");
            Expect("admin tries to pick up an item", () => _game.PickUpItem(1), "PickUpStatus", "ADMIN_HAS_NO_INVENTORY");
            Expect("offline player tries to pick up an item", () => _game.PickUpItem(5), "PickUpStatus", "NOT_ONLINE");
            Sql("UPDATE dbo.Player SET IsOnline = 1 WHERE PlayerID = 5;");
            Expect("online player has no current tile", () => _game.PickUpItem(5), "PickUpStatus", "NO_CURRENT_TILE");
        }

        // npc movement, damage and respawn tests

        private void NpcTests()
        {
            Expect("NPC moves one step", () => _game.MoveNpc(1), "MoveNpcStatus", "SUCCESS");
            Expect("move an npc that doesnt exist", () => _game.MoveNpc(999), "MoveNpcStatus", "NPC_NOT_FOUND");
            Expect("respawn an npc that is still alive", () => _game.RespawnNpc(), "RespawnStatus", "ALREADY_ALIVE");

            int? healthLeft;
            bool killed = _game.DamageNpc(1, 40, out healthLeft);
            ExpectValue("NPC has 960 health after taking damage", () => healthLeft, 960);
            ExpectValue("NPC survives the hit", () => killed, false);

            Sql("UPDATE dbo.NPC SET Health = 50 WHERE NPCID = 1;");
            killed = _game.DamageNpc(1, 60, out healthLeft);
            ExpectValue("NPC health to 0", () => healthLeft, 0);
            ExpectValue("NPC dies from the hit", () => killed, true);
            ExpectValue("NPC is marked as dead", () => Scalar("SELECT IsAlive FROM dbo.NPC WHERE NPCID = 1"), false);
            ExpectValue("NPC drops an item on its tile", () => Scalar("SELECT COUNT(*) FROM dbo.Item i JOIN dbo.NPC n ON n.TileID = i.TileID WHERE n.NPCID = 1"), 1);

            Expect("try to move a dead NPC", () => _game.MoveNpc(1), "MoveNpcStatus", "NPC_DEAD");
            Expect("try to respawn the NPC before the one-minute timer ends", () => _game.RespawnNpc(), "RespawnStatus", "NOT_READY");
            Sql("UPDATE dbo.NPC SET RespawnTime = DATEADD(MINUTE, -1, GETDATE()) WHERE NPCID = 1;");
            Expect("respawn the NPC after the timer ends", () => _game.RespawnNpc(), "RespawnStatus", "SUCCESS");
            Expect("try to respawn the NPC again straight away", () => _game.RespawnNpc(), "RespawnStatus", "ALREADY_ALIVE");
            ExpectValue("respawned NPC has full health", () => Scalar("SELECT Health FROM dbo.NPC WHERE NPCID = 1"), 1000);
        }

        // Ending a game and logging out

        private void KillGameTests()
        {
            Sql("UPDATE dbo.Player SET IsOnline = 1 WHERE PlayerID IN (1, 2, 3);");
            Expect("regular player tries to end the game", () => _admin.KillRunningGame(2), "KillStatus", "NOT_ADMIN");
            Expect("admin doesnt exist", () => _admin.KillRunningGame(999), "KillStatus", "ADMIN_NOT_FOUND");
            DataTable kill = Expect("admin ends the game", () => _admin.KillRunningGame(1), "KillStatus", "SUCCESS");
            ExpectValue("two players are logged out", () => kill.Rows[0]["PlayersLoggedOut"], 2);
            ExpectValue("only the admin remains online", () => Scalar("SELECT COUNT(*) FROM dbo.Player WHERE IsOnline = 1"), 1);

            Sql("UPDATE dbo.Player SET IsOnline = 1, IsInvincible = 1, InvincibleUntil = NULL WHERE PlayerID = 2;");
            Expect("log out a player", () => _login.Logout(2), "LogoutStatus", "SUCCESS");
            ExpectValue("logging out clears invincibility", () => Scalar("SELECT IsInvincible FROM dbo.Player WHERE PlayerID = 2"), false);
            Expect("log out a player that doesnt exist", () => _login.Logout(999), "LogoutStatus", "PLAYER_NOT_FOUND");
        }

        // Admin player management tests

        private void AddPlayerTests()
        {
            Expect("admin adds another admin", () => _admin.AddPlayer(1, "secondadmin@example.com", "SecondAdmin", "AdminPassword456!", true), "AddPlayerStatus", "SUCCESS");
            Expect("admin adds a regular player", () => _admin.AddPlayer(1, "yes@example.com", "YesGoose", "sadfAsdfas@3!", false), "AddPlayerStatus", "SUCCESS");
            ExpectValue("new admin is saved as an admin", () => Scalar("SELECT IsAdmin FROM dbo.Player WHERE Username = 'SecondAdmin'"), true);
            Expect("try to use a username thats already taken", () => _admin.AddPlayer(1, "yesyes@example.com", "ZandiGoose", "SkiBidi1!", false), "AddPlayerStatus", "USERNAME_TAKEN");
            Expect("try to use a email thats already taken", () => _admin.AddPlayer(1, "zandi@example.com", "DinGoose", "chungus12A!", false), "AddPlayerStatus", "EMAIL_TAKEN");
            Expect("add a player with a blank username", () => _admin.AddPlayer(1, "what@example.com", "", "adsfasdf1A!", false), "AddPlayerStatus", "MISSING_FIELDS");
            Expect("regular player tries to add another player", () => _admin.AddPlayer(2, "sneaky@example.com", "SneakyGoose", "asdfasdfA1!", false), "AddPlayerStatus", "NOT_ADMIN");
            Expect("requesting admin doesnt exist", () => _admin.AddPlayer(999, "ghost@example.com", "GhostGoose", "asdfasdfA2!", false), "AddPlayerStatus", "ADMIN_NOT_FOUND");
        }

        private void UpdatePlayerTests()
        {
            Expect("change McPlayers username and email", () => _admin.UpdatePlayer(1, 3, "mcplayer2@example.com", "Mcplayerington Two", "Passw0rd!", 5, false, false), "UpdatePlayerStatus", "SUCCESS");
            ExpectValue("new username is saved", () => Scalar("SELECT Username FROM dbo.Player WHERE PlayerID = 3"), "Mcplayerington Two");

            Expect("unlock LockedGoose's account", () => _admin.UpdatePlayer(1, 4, "lockedout@example.com", "LockedGoose", "wrongpass", 0, false, false), "UpdatePlayerStatus", "SUCCESS");
            Expect("unlocked player can log in again", () => _login.Login("LockedGoose", "wrongpass"), "LoginStatus", "SUCCESS");
            ExpectValue("unlocking resets failed login attempts", () => Scalar("SELECT FailedLoginAttempts FROM dbo.Player WHERE PlayerID = 4"), 0);

            Expect("change to a username thats already taken", () => _admin.UpdatePlayer(1, 3, "mcplayer2@example.com", "ZandiGoose", "Passw0rd!", 5, false, false), "UpdatePlayerStatus", "USERNAME_TAKEN");
            Expect("change to an email thats already taken", () => _admin.UpdatePlayer(1, 3, "zandi@example.com", "Mcplayerington Two", "Passw0rd!", 5, false, false), "UpdatePlayerStatus", "EMAIL_TAKEN");
            Expect("try to set a negative score", () => _admin.UpdatePlayer(1, 3, "mcplayer2@example.com", "Mcplayerington Two", "Passw0rd!", -5, false, false), "UpdatePlayerStatus", "INVALID_SCORE");
            Expect("try to set a blank username", () => _admin.UpdatePlayer(1, 3, "mcplayer2@example.com", "", "Passw0rd!", 5, false, false), "UpdatePlayerStatus", "MISSING_FIELDS");
            Expect("update a player that doesnt exist", () => _admin.UpdatePlayer(1, 999, "x@example.com", "X", "x", 0, false, false), "UpdatePlayerStatus", "PLAYER_NOT_FOUND");
            Expect("regular player tries to update another player", () => _admin.UpdatePlayer(2, 3, "mcplayer2@example.com", "Mcplayerington Two", "Passw0rd!", 5, false, false), "UpdatePlayerStatus", "NOT_ADMIN");
        }

        private void DeletePlayerTests()
        {
            Expect("regular player tries to delete someone else", () => _admin.DeletePlayer(4, 5), "DeletePlayerStatus", "NOT_AUTHORISED");
            Expect("requesting player doesnt exist", () => _admin.DeletePlayer(999, 5), "DeletePlayerStatus", "REQUESTER_NOT_FOUND");
            Expect("try to delete a player that doesnt exist", () => _admin.DeletePlayer(1, 999), "DeletePlayerStatus", "PLAYER_NOT_FOUND");
            Expect("admin deletes McPlayer", () => _admin.DeletePlayer(1, 3), "DeletePlayerStatus", "SUCCESS");
            Expect("ZandiGoose deletes  own account", () => _admin.DeletePlayer(2, 2), "DeletePlayerStatus", "SUCCESS");

            ExpectValue("both players have been deleted", () => Scalar("SELECT COUNT(*) FROM dbo.Player WHERE PlayerID IN (2, 3)"), 0);
            ExpectValue("their nests have been deleted", () => Scalar("SELECT COUNT(*) FROM dbo.Nest"), 0);
            ExpectValue("only the two items on tiles remain", () => Scalar("SELECT COUNT(*) FROM dbo.Item"), 2);
            ExpectValue("their chat messages have been deleted", () => Scalar("SELECT COUNT(*) FROM dbo.ChatMessage"), 0);
        }

        // Helper methods

        private void RunGroup(string title, Action body)
        {
            _log.AppendLine();
            _log.AppendLine("=== " + title + " ===");
            try
            {
                // Reset the database so groups starts with same test data.
                Sql("EXEC dbo.usp_CreateGooseFighterDatabase;");
                body();
            }
            catch (Exception ex)
            {
                Record("group setup/run", false, "exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Calls a DAO method and checks the status returned in its first row.
        /// </summary>
        private DataTable Expect(string label, Func<DataTable> call, string statusColumn, string expected)
        {
            try
            {
                DataTable table = call();
                string actual = table.Rows.Count > 0 ? Convert.ToString(table.Rows[0][statusColumn]) : "<no rows>";
                Record(label, actual == expected, actual == expected ? actual : actual + "  (expected " + expected + ")");
                return table;
            }
            catch (Exception ex)
            {
                Record(label, false, "exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Checks a returned value or database value against what expected.
        /// </summary>
        private void ExpectValue(string label, Func<object> actual, object expected)
        {
            try
            {
                string a = Convert.ToString(actual());
                string e = Convert.ToString(expected);
                Record(label, a == e, a == e ? a : a + "  (expected " + e + ")");
            }
            catch (Exception ex)
            {
                Record(label, false, "exception: " + ex.Message);
            }
        }

        private void Record(string label, bool passed, string detail)
        {
            if (passed) _passed++; else _failed++;
            _log.AppendLine((passed ? "PASS  " : "FAIL  ") + label + " -> " + detail);
        }
        
        private object Scalar(string sql)
        {
            using (SqlConnection connection = _db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                return command.ExecuteScalar();
            }
        }

        private void Sql(string sql)
        {
            using (SqlConnection connection = _db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}