/* ============================================================
   TSQL procedures for the 12 CRUD processes from the CRUD table. 
   Instead of the tests from DAT602_assignment_1_test_procedures.sql,
   this one has the actual business logic.

   DAT602_Assignment_1_DDL_Script.sql should be run first to set everything up.
   ============================================================ */

USE GooseFighterDB;
GO


/* ------------------------------------------------------------
   CRUD Process 1: Player login and including lockout. Replaces 
   usp_TestLogin. lockout logic is here now instead of in C# form.
   ------------------------------------------------------------ */
CREATE OR ALTER PROCEDURE dbo.usp_LoginPlayer
    @Username VARCHAR(100),
    @Password VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PlayerID INT, @StoredPassword VARCHAR(100),
            @IsLockedOut BIT, @IsAdmin BIT, @FailedAttempts INT;

    SELECT
        @PlayerID       = PlayerID,
        @StoredPassword = Password,
        @IsLockedOut    = IsLockedOut,
        @IsAdmin        = IsAdmin,
        @FailedAttempts = FailedLoginAttempts
    FROM dbo.Player
    WHERE Username = @Username;

    --if no matching username
    IF @PlayerID IS NULL
    BEGIN
        SELECT CAST(NULL AS INT) AS PlayerID, CAST(NULL AS BIT) AS IsAdmin, 'NOT_FOUND' AS LoginStatus;
        RETURN;
    END

    --already locked out means don't check the password
    IF @IsLockedOut = 1
    BEGIN
        SELECT @PlayerID AS PlayerID, @IsAdmin AS IsAdmin, 'LOCKED_OUT' AS LoginStatus;
        RETURN;
    END

    --wrong password means add to the counter and lock out if the counter gets to 5
    IF @StoredPassword <> @Password
    BEGIN
        UPDATE dbo.Player
        SET FailedLoginAttempts = FailedLoginAttempts + 1,
            IsLockedOut = CASE WHEN FailedLoginAttempts + 1 >= 5 THEN 1 ELSE 0 END
        WHERE PlayerID = @PlayerID;

        SELECT @PlayerID AS PlayerID, @IsAdmin AS IsAdmin,
               CASE WHEN @FailedAttempts + 1 >= 5 THEN 'LOCKED_OUT' ELSE 'INCORRECT_PASSWORD' END AS LoginStatus;
        RETURN;
    END

    --correct password means reset the counter and change their state to online
    UPDATE dbo.Player
    SET FailedLoginAttempts = 0,
        IsOnline = 1
    WHERE PlayerID = @PlayerID;

    SELECT @PlayerID AS PlayerID, @IsAdmin AS IsAdmin, 'SUCCESS' AS LoginStatus;
END
GO

--test calls for each case using usp_LoginPlayer
EXEC dbo.usp_LoginPlayer @Username = 'ZandiGoose', @Password = 'Password123'; --SUCCESS
EXEC dbo.usp_LoginPlayer @Username = 'ZandiGoose', @Password = 'wrongpassXD'; --INCORRECT_PASSWORD
EXEC dbo.usp_LoginPlayer @Username = 'LockedGoose', @Password = 'wrongpassxd'; --LOCKED_OUT (already locked)
EXEC dbo.usp_LoginPlayer @Username = 'NotArealPlayer', @Password = 'skibidi'; --NOT_FOUND
GO


/* ------------------------------------------------------------
   CRUD Process 2: Player registration.
   ------------------------------------------------------------ */
CREATE OR ALTER PROCEDURE dbo.usp_RegisterPlayer
    @Email    VARCHAR(255),
    @Username VARCHAR(100),
    @Password VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Player WHERE Username = @Username)
    BEGIN
        SELECT CAST(NULL AS INT) AS PlayerID, 'USERNAME_TAKEN' AS RegisterStatus;
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.Player WHERE Email = @Email)
    BEGIN
        SELECT CAST(NULL AS INT) AS PlayerID, 'EMAIL_TAKEN' AS RegisterStatus;
        RETURN;
    END

    --successful registration is like a successful login, so the new player starts online.
    INSERT INTO dbo.Player (Email, Username, Password, Score, IsAdmin, IsOnline)
    VALUES (@Email, @Username, @Password, 0, 0, 1);

    DECLARE @NewPlayerID INT = SCOPE_IDENTITY();

    SELECT @NewPlayerID AS PlayerID, 'SUCCESS' AS RegisterStatus;
END
GO

--test calls for each case using usp_RegisterPlayer
EXEC dbo.usp_RegisterPlayer @Email = 'newguyforreal@example.com', @Username = 'NewPlayer1', @Password = 'Passwooord!1'; --SUCCESS
EXEC dbo.usp_RegisterPlayer @Email = 'another@example.com', @Username = 'ZandiGoose', @Password = 'Passwoooadsfafrd!2'; --USERNAME_TAKEN
EXEC dbo.usp_RegisterPlayer @Email = 'zandi@example.com', @Username = 'SkibidiPlayer', @Password = 'Password1!'; --EMAIL_TAKEN
GO

/* ------------------------------------------------------------
   CRUD Process 3: Laying out tiles on a game board.
   Admins extends the board by placing a tile next to an existing
   one or removes an empty tile with the rubbish bin tool.
   The tests below assume board is 5x5
   ------------------------------------------------------------ */
CREATE OR ALTER PROCEDURE dbo.usp_PlaceTile
    @XPosition INT,
    @YPosition INT
AS
BEGIN
    SET NOCOUNT ON;

    --a tile is already on these coords
    IF EXISTS (SELECT 1 FROM dbo.Tile WHERE XPosition = @XPosition AND YPosition = @YPosition)
    BEGIN
        SELECT CAST(NULL AS INT) AS TileID, 'TILE_EXISTS' AS PlaceTileStatus;
        RETURN;
    END

    --new tiles need to touch an already existing tile
    --the first tile on an empty board is the exception.
    IF EXISTS (SELECT 1 FROM dbo.Tile)
       AND NOT EXISTS (
            SELECT 1 FROM dbo.Tile
            WHERE (XPosition = @XPosition - 1 AND YPosition = @YPosition)
               OR (XPosition = @XPosition + 1 AND YPosition = @YPosition)
               OR (XPosition = @XPosition AND YPosition = @YPosition - 1)
               OR (XPosition = @XPosition AND YPosition = @YPosition + 1))
    BEGIN
        SELECT CAST(NULL AS INT) AS TileID, 'NOT_ADJACENT' AS PlaceTileStatus;
        RETURN;
    END

    INSERT INTO dbo.Tile (XPosition, YPosition) VALUES (@XPosition, @YPosition);

    SELECT SCOPE_IDENTITY() AS TileID, 'SUCCESS' AS PlaceTileStatus;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_DeleteTile
    @TileID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Tile WHERE TileID = @TileID)
    BEGIN
        SELECT 'TILE_NOT_FOUND' AS DeleteTileStatus;
        RETURN;
    END

    --a tile can only be removed when nothing is on it
    IF EXISTS (SELECT 1 FROM dbo.Player WHERE CurrentTileID = @TileID)
       OR EXISTS (SELECT 1 FROM dbo.Nest WHERE TileID = @TileID)
       OR EXISTS (SELECT 1 FROM dbo.NPC WHERE TileID = @TileID)
       OR EXISTS (SELECT 1 FROM dbo.Item WHERE TileID = @TileID)
    BEGIN
        SELECT 'TILE_IN_USE' AS DeleteTileStatus;
        RETURN;
    END

    DELETE FROM dbo.Tile WHERE TileID = @TileID;

    SELECT 'SUCCESS' AS DeleteTileStatus;
END
GO

--test calls
EXEC dbo.usp_PlaceTile @XPosition = 5, @YPosition = 2; --SUCCESS (touches tile (4,2)); becomes TileID 26
EXEC dbo.usp_PlaceTile @XPosition = 0, @YPosition = 0; --TILE_EXISTS
EXEC dbo.usp_PlaceTile @XPosition = 9, @YPosition = 9; --NOT_ADJACENT
EXEC dbo.usp_DeleteTile @TileID = 13; --TILE_IN_USE (player + nest on it)
EXEC dbo.usp_DeleteTile @TileID = 19; --TILE_IN_USE (watermelon on it)
EXEC dbo.usp_DeleteTile @TileID = 999; --TILE_NOT_FOUND
EXEC dbo.usp_DeleteTile @TileID = 26; --SUCCESS (the tile just placed, still empty)
GO


/* ------------------------------------------------------------
   CRUD Process 4: Placing an item on a tile.
   Admin picks an item type and clicks an empty tile,
   the rubbish bin tool on a tile that has an item removes that item.
   ------------------------------------------------------------ */
CREATE OR ALTER PROCEDURE dbo.usp_PlaceItem
    @ItemTypeID INT,
    @TileID     INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.ItemType WHERE ItemTypeID = @ItemTypeID)
    BEGIN
        SELECT CAST(NULL AS INT) AS ItemID, 'ITEMTYPE_NOT_FOUND' AS PlaceItemStatus;
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Tile WHERE TileID = @TileID)
    BEGIN
        SELECT CAST(NULL AS INT) AS ItemID, 'TILE_NOT_FOUND' AS PlaceItemStatus;
        RETURN;
    END

    --only one item per tile
    IF EXISTS (SELECT 1 FROM dbo.Item WHERE TileID = @TileID)
    BEGIN
        SELECT CAST(NULL AS INT) AS ItemID, 'TILE_HAS_ITEM' AS PlaceItemStatus;
        RETURN;
    END

    --pellet guns start with random 3-10 ammo, everything else has none
    DECLARE @Ammo INT = NULL;
    IF EXISTS (SELECT 1 FROM dbo.ItemType WHERE ItemTypeID = @ItemTypeID AND Name = 'Pellet Gun')
        SET @Ammo = (ABS(CHECKSUM(NEWID())) % 8) + 3;

    INSERT INTO dbo.Item (ItemTypeID, TileID, Ammo)
    VALUES (@ItemTypeID, @TileID, @Ammo);

    SELECT SCOPE_IDENTITY() AS ItemID, 'SUCCESS' AS PlaceItemStatus;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_RemoveItemFromTile
    @TileID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Item WHERE TileID = @TileID)
    BEGIN
        SELECT 'NO_ITEM' AS RemoveItemStatus;
        RETURN;
    END

    DELETE FROM dbo.Item WHERE TileID = @TileID;

    SELECT 'SUCCESS' AS RemoveItemStatus;
END
GO

--test calls
EXEC dbo.usp_PlaceItem @ItemTypeID = 6,  @TileID = 20; --SUCCESS (banana on empty tile 20)
EXEC dbo.usp_PlaceItem @ItemTypeID = 2,  @TileID = 21; --SUCCESS (pellet gun)
EXEC dbo.usp_PlaceItem @ItemTypeID = 6,  @TileID = 19; --TILE_HAS_ITEM (watermelon already there)
EXEC dbo.usp_PlaceItem @ItemTypeID = 99, @TileID = 22; --ITEMTYPE_NOT_FOUND
EXEC dbo.usp_PlaceItem @ItemTypeID = 6,  @TileID = 999; --TILE_NOT_FOUND
EXEC dbo.usp_RemoveItemFromTile @TileID = 20; --SUCCESS (removes the banana)
EXEC dbo.usp_RemoveItemFromTile @TileID = 20; --NO_ITEM (already gone)
SELECT ItemID, ItemTypeID, TileID, Ammo FROM dbo.Item WHERE TileID = 21; --confirm ammo is between 3 and 10
GO

/* ------------------------------------------------------------
   CRUD Process 5: Player game play movement (existing tiles only).
   The game is played with arrow keys or wasd, so the caller passes a
   direction rather than a tile. The database works out the target
   tile and decides whether the move is allowed. Current coords are X and Y, so
   UP = Y - 1, DOWN = Y + 1, LEFT = X - 1, RIGHT = X + 1.
   Result set has NewTileID, NewX, NewY, MoveStatus.
   ------------------------------------------------------------ */
CREATE OR ALTER PROCEDURE dbo.usp_MovePlayer
    @PlayerID  INT,
    @Direction VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Direction IS NULL OR @Direction NOT IN ('UP', 'DOWN', 'LEFT', 'RIGHT')
    BEGIN
        SELECT NULL AS NewTileID, NULL AS NewX, NULL AS NewY, 'INVALID_DIRECTION' AS MoveStatus;
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Player WHERE PlayerID = @PlayerID)
    BEGIN
        SELECT NULL AS NewTileID, NULL AS NewX, NULL AS NewY, 'PLAYER_NOT_FOUND' AS MoveStatus;
        RETURN;
    END

    DECLARE @IsOnline BIT, @CurrentTileID INT;
    SELECT @IsOnline = IsOnline, @CurrentTileID = CurrentTileID
    FROM dbo.Player
    WHERE PlayerID = @PlayerID;

    IF @IsOnline = 0
    BEGIN
        SELECT NULL AS NewTileID, NULL AS NewX, NULL AS NewY, 'NOT_ONLINE' AS MoveStatus;
        RETURN;
    END

    IF @CurrentTileID IS NULL
    BEGIN
        SELECT NULL AS NewTileID, NULL AS NewX, NULL AS NewY, 'NO_CURRENT_TILE' AS MoveStatus;
        RETURN;
    END

    --make the direction into a step from the position on the grid
    DECLARE @DX INT = CASE @Direction WHEN 'LEFT' THEN -1 WHEN 'RIGHT' THEN 1 ELSE 0 END;
    DECLARE @DY INT = CASE @Direction WHEN 'UP'   THEN -1 WHEN 'DOWN'  THEN 1 ELSE 0 END;

    --find the specific the tile which is the specific step from before away from the player's current tile
    DECLARE @TargetTileID INT, @NewX INT, @NewY INT;
    SELECT @TargetTileID = t2.TileID, @NewX = t2.XPosition, @NewY = t2.YPosition
    FROM dbo.Tile t1
    JOIN dbo.Tile t2 ON t2.XPosition = t1.XPosition + @DX AND t2.YPosition = t1.YPosition + @DY
    WHERE t1.TileID = @CurrentTileID;

    --no tile there (not on board)
    IF @TargetTileID IS NULL
    BEGIN
        SELECT NULL AS NewTileID, NULL AS NewX, NULL AS NewY, 'OFF_BOARD' AS MoveStatus;
        RETURN;
    END

    --only one player per tile, but offline players don't count, they are not
    --on the map until they log back in. the npc also blocks the tile.
    IF EXISTS (SELECT 1 FROM dbo.Player WHERE CurrentTileID = @TargetTileID AND IsOnline = 1 AND PlayerID <> @PlayerID)
       OR EXISTS (SELECT 1 FROM dbo.NPC WHERE TileID = @TargetTileID AND IsAlive = 1)
    BEGIN
        SELECT NULL AS NewTileID, NULL AS NewX, NULL AS NewY, 'TILE_OCCUPIED' AS MoveStatus;
        RETURN;
    END

    UPDATE dbo.Player SET CurrentTileID = @TargetTileID WHERE PlayerID = @PlayerID;

    SELECT @TargetTileID AS NewTileID, @NewX AS NewX, @NewY AS NewY, 'SUCCESS' AS MoveStatus;
END
GO

--test calls    
--initial positions are ZandiGoose (PlayerID 2) being online on tile 13 (2,2).
EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'UP'; --SUCCESS: tile 12 (2,1)
EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'UP'; --TILE_OCCUPIED: the NPC is on tile 11 (2,0)
EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'LEFT'; --SUCCESS: tile 7 = (1,1); a player is there but offline so it doesn't stop it

--test setup, McPlayer (PlayerID 3) online on tile 8 (1,2)
UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 8 WHERE PlayerID = 3;

EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'DOWN'; --TILE_OCCUPIED: McPlayer is online on tile 8
EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'UP'; --SUCCESS: tile 6 (1,0), item on tile doesnt stop movement
EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'LEFT'; --SUCCESS: tile 1 (0,0)
EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'LEFT'; --OFF_BOARD: nothing at(-1,0)
EXEC dbo.usp_MovePlayer @PlayerID = 2, @Direction = 'SKIBIDI'; --INVALID_DIRECTION
EXEC dbo.usp_MovePlayer @PlayerID = 991, @Direction = 'UP'; --PLAYER_NOT_FOUND
EXEC dbo.usp_MovePlayer @PlayerID = 5, @Direction = 'UP'; --NOT_ONLINE 
EXEC dbo.usp_MovePlayer @PlayerID = 6, @Direction = 'UP'; --NO_CURRENT_TILE (player made by the registration test)
GO



/* ------------------------------------------------------------
   CRUD Process 6: Gameplay scoring.
   Players get 1 point for defeating another player
   nobody ever loses score, and defeating the NPC doesnt give score.
   An attack hits the tile in front of the attacker, so the
   caller passes the direction the goose is facing
   Damage: 40, or x1.5 with a Baseball Bat, x0.75 with a Shield, 
   and x0.5 if the player getting attacked has a Shield
   Pellet Gun and Grenade are ranged items, so for this they arent
   seen an an item (basic 40 damage).
   Result set has AttackStatus, DamageDealt, VictimPlayerID, VictimHealth, AttackerScore.
   ------------------------------------------------------------ */
CREATE OR ALTER PROCEDURE dbo.usp_Attack
    @AttackerID INT,
    @Direction  VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
 
    DECLARE @BaseDamage INT = 40;
    DECLARE @MaxHealth  INT = 4000;
 
    IF @Direction IS NULL OR @Direction NOT IN ('UP', 'DOWN', 'LEFT', 'RIGHT')
    BEGIN
        SELECT 'INVALID_DIRECTION' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    DECLARE @IsOnline BIT, @IsAdmin BIT, @AttackerTileID INT;
    SELECT @IsOnline = IsOnline, @IsAdmin = IsAdmin, @AttackerTileID = CurrentTileID
    FROM dbo.Player
    WHERE PlayerID = @AttackerID;
 
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT 'ATTACKER_NOT_FOUND' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    --admins can't attack
    IF @IsAdmin = 1
    BEGIN
        SELECT 'ATTACKER_IS_ADMIN' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    IF @IsOnline = 0
    BEGIN
        SELECT 'NOT_ONLINE' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    IF @AttackerTileID IS NULL
    BEGIN
        SELECT 'NO_CURRENT_TILE' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    --find the tile in front of the attacker
    DECLARE @DX INT = CASE @Direction WHEN 'LEFT' THEN -1 WHEN 'RIGHT' THEN 1 ELSE 0 END;
    DECLARE @DY INT = CASE @Direction WHEN 'UP'   THEN -1 WHEN 'DOWN'  THEN 1 ELSE 0 END;
 
    DECLARE @TargetTileID INT;
    SELECT @TargetTileID = t2.TileID
    FROM dbo.Tile t1
    JOIN dbo.Tile t2 ON t2.XPosition = t1.XPosition + @DX
                    AND t2.YPosition = t1.YPosition + @DY
    WHERE t1.TileID = @AttackerTileID;
 
    IF @TargetTileID IS NULL
    BEGIN
        SELECT 'NOTHING_THERE' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    --who is standing on that tile? (online, non admins only)
    --invincibility counts only while active. (InvincibleUntil)
    --NULL means invincible until changed state (e.g. in the settings screen).
    DECLARE @VictimID INT, @VictimHealth INT, @VictimInvincible BIT;
    SELECT @VictimID = PlayerID,
           @VictimHealth = Health,
           @VictimInvincible = CASE WHEN IsInvincible = 1
                                     AND (InvincibleUntil IS NULL OR InvincibleUntil > GETDATE())
                                    THEN 1 ELSE 0 END
    FROM dbo.Player
    WHERE CurrentTileID = @TargetTileID AND IsOnline = 1 AND IsAdmin = 0 AND PlayerID <> @AttackerID;
 
    IF @VictimID IS NULL
    BEGIN
        --no player there. If it's the NPC, the NPC procedure (process 8) handles it.
        IF EXISTS (SELECT 1 FROM dbo.NPC WHERE TileID = @TargetTileID AND IsAlive = 1)
            SELECT 'NPC_IN_FRONT' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        ELSE
            SELECT 'NOTHING_THERE' AS AttackStatus, NULL AS DamageDealt, NULL AS VictimPlayerID, NULL AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    IF @VictimInvincible = 1
    BEGIN
        SELECT 'TARGET_INVINCIBLE' AS AttackStatus, NULL AS DamageDealt, @VictimID AS VictimPlayerID, @VictimHealth AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    --a goose standing on its nest cant be attacked
    IF EXISTS (SELECT 1 FROM dbo.Nest WHERE PlayerID = @VictimID AND TileID = @TargetTileID)
    BEGIN
        SELECT 'TARGET_ON_NEST' AS AttackStatus, NULL AS DamageDealt, @VictimID AS VictimPlayerID, @VictimHealth AS VictimHealth, NULL AS AttackerScore;
        RETURN;
    END
 
    --damage, base value gets changed by the attacker's and the defender's items
    DECLARE @OutMult DECIMAL(4,2) = 1.00;
    DECLARE @InMult  DECIMAL(4,2) = 1.00;
 
    IF EXISTS (SELECT 1 FROM dbo.Item i JOIN dbo.ItemType it ON it.ItemTypeID = i.ItemTypeID
               WHERE i.HolderPlayerID = @AttackerID AND it.Name = 'Baseball Bat')
    BEGIN
        SET @OutMult = 1.50;
    END
    ELSE IF EXISTS (SELECT 1 FROM dbo.Item i JOIN dbo.ItemType it ON it.ItemTypeID = i.ItemTypeID
                    WHERE i.HolderPlayerID = @AttackerID AND it.Name = 'Shield')
    BEGIN
        SET @OutMult = 0.75;
    END
 
    IF EXISTS (SELECT 1 FROM dbo.Item i JOIN dbo.ItemType it ON it.ItemTypeID = i.ItemTypeID
               WHERE i.HolderPlayerID = @VictimID AND it.Name = 'Shield')
    BEGIN
        SET @InMult = 0.50;
    END
 
    DECLARE @Damage INT = CAST(ROUND(@BaseDamage * @OutMult * @InMult, 0) AS INT);
 
    --survived hit means lose health then get 0.5 seconds of invincibility
    IF @VictimHealth - @Damage > 0
    BEGIN
        UPDATE dbo.Player
        SET Health = Health - @Damage,
            IsInvincible = 1,
            InvincibleUntil = DATEADD(MILLISECOND, 500, GETDATE())
        WHERE PlayerID = @VictimID;
 
        SELECT 'HIT' AS AttackStatus, @Damage AS DamageDealt, @VictimID AS VictimPlayerID,
               @VictimHealth - @Damage AS VictimHealth,
               (SELECT Score FROM dbo.Player WHERE PlayerID = @AttackerID) AS AttackerScore;
        RETURN;
    END
 
    --lethal hit, The attacker scores a point.
    UPDATE dbo.Player SET Score = Score + 1 WHERE PlayerID = @AttackerID;
 
    --the victim's nest despawns and everything in it is lost.
    --the item in the victim's beak is lost too.
    DELETE FROM dbo.Item
    WHERE HolderPlayerID = @VictimID
       OR NestID IN (SELECT NestID FROM dbo.Nest WHERE PlayerID = @VictimID);
    DELETE FROM dbo.Nest WHERE PlayerID = @VictimID;
 
    --respawn on a random free tile somewhere else on the map
    --not the death tile, and no online player, nest, npc or item on it
    DECLARE @SpawnTileID INT;
    SELECT TOP 1 @SpawnTileID = t.TileID
    FROM dbo.Tile t
    WHERE t.TileID <> @TargetTileID
      AND NOT EXISTS (SELECT 1 FROM dbo.Player p WHERE p.CurrentTileID = t.TileID AND p.IsOnline = 1)
      AND NOT EXISTS (SELECT 1 FROM dbo.Nest   n WHERE n.TileID = t.TileID)
      AND NOT EXISTS (SELECT 1 FROM dbo.NPC    c WHERE c.TileID = t.TileID AND c.IsAlive = 1)
      AND NOT EXISTS (SELECT 1 FROM dbo.Item   i WHERE i.TileID = t.TileID)
    ORDER BY NEWID();
 
    --if the board is completely full, respawn where they fell
    SET @SpawnTileID = ISNULL(@SpawnTileID, @TargetTileID);
 
    --new nest under them, full health, 5 seconds invincibility
    INSERT INTO dbo.Nest (PlayerID, TileID) VALUES (@VictimID, @SpawnTileID);
 
    UPDATE dbo.Player
    SET Health = @MaxHealth,
        CurrentTileID = @SpawnTileID,
        IsInvincible = 1,
        InvincibleUntil = DATEADD(SECOND, 5, GETDATE())
    WHERE PlayerID = @VictimID;
 
    SELECT 'KILLED' AS AttackStatus, @Damage AS DamageDealt, @VictimID AS VictimPlayerID,
           @MaxHealth AS VictimHealth,
           (SELECT Score FROM dbo.Player WHERE PlayerID = @AttackerID) AS AttackerScore;
END
GO
 
--test setup: ZandiGoose (PlayerID 2, holds Baseball Bat) on tile 13 (2,2), standing on his nest.
--McPlayer (PlayerID 3) on tile 14 (2,3), directly below him. Both online, full health.
UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 13, Health = 4000, IsInvincible = 0, InvincibleUntil = NULL WHERE PlayerID = 2;
UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 14, Health = 4000, IsInvincible = 0, InvincibleUntil = NULL WHERE PlayerID = 3;
 
EXEC dbo.usp_Attack @AttackerID = 2, @Direction = 'DOWN'; --HIT: bat does 60 damage, McPlayer health 3940
EXEC dbo.usp_Attack @AttackerID = 2, @Direction = 'DOWN'; --TARGET_INVINCIBLE: still inside the 0.5 seconds
WAITFOR DELAY '00:00:01';
EXEC dbo.usp_Attack @AttackerID = 2, @Direction = 'DOWN'; --HIT again: McPlayer health 3880
EXEC dbo.usp_Attack @AttackerID = 3, @Direction = 'UP'; --TARGET_ON_NEST: ZandiGoose is standing on his nest
 
--test setup: McPlayer is nearly dead
UPDATE dbo.Player SET Health = 50, IsInvincible = 0, InvincibleUntil = NULL WHERE PlayerID = 3;
EXEC dbo.usp_Attack @AttackerID = 2, @Direction = 'DOWN'; --KILLED: Zandi's score added 1 (3 to 4), McPlayer respawns
SELECT PlayerID, Username, Score, Health, CurrentTileID, IsInvincible, InvincibleUntil
FROM dbo.Player WHERE PlayerID IN (2, 3); --McPlayer: health 4000, new tile, invincible for 5 seconds
SELECT NestID, PlayerID, TileID FROM dbo.Nest; --McPlayer's old nest (tile 7) is gone, a new one spawned
 
--other scenarios
UPDATE dbo.Player SET CurrentTileID = 12 WHERE PlayerID = 2;
EXEC dbo.usp_Attack @AttackerID = 2, @Direction = 'UP'; --NPC_IN_FRONT: the NPC is on tile 11 (2,0)
UPDATE dbo.Player SET CurrentTileID = 1 WHERE PlayerID = 2;
EXEC dbo.usp_Attack @AttackerID = 2, @Direction = 'LEFT'; --NOTHING_THERE: edge of the board
EXEC dbo.usp_Attack @AttackerID = 2, @Direction = 'SIDEWAYS'; --INVALID_DIRECTION
EXEC dbo.usp_Attack @AttackerID = 999, @Direction = 'UP'; --ATTACKER_NOT_FOUND
EXEC dbo.usp_Attack @AttackerID = 1, @Direction = 'UP'; --ATTACKER_IS_ADMIN (AdminGoose)
EXEC dbo.usp_Attack @AttackerID = 5, @Direction = 'UP'; --NOT_ONLINE (BrandNewGoose)
EXEC dbo.usp_Attack @AttackerID = 6, @Direction = 'UP'; --NO_CURRENT_TILE (TestNewPlayer1)
GO



/* ------------------------------------------------------------
   CRUD Process 7: Player game play acquiring inventory.
   A goose has 1 inventory slot (the item in its beak) plus its
   nest, which holds up to 9 items.
   usp_PickUpItem, take the item lying on the player's own tile
   usp_DropItem, on their nest means store it there, anywhere else means it disappears
   usp_TakeFromNest, move an item from the nest into the beak
   Admins have no inventory
   ------------------------------------------------------------ */
CREATE OR ALTER PROCEDURE dbo.usp_PickUpItem
    @PlayerID INT
AS
BEGIN
    SET NOCOUNT ON;
 
    DECLARE @IsOnline BIT, @IsAdmin BIT, @TileID INT;
    SELECT @IsOnline = IsOnline, @IsAdmin = IsAdmin, @TileID = CurrentTileID
    FROM dbo.Player
    WHERE PlayerID = @PlayerID;
 
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT 'PLAYER_NOT_FOUND' AS PickUpStatus, NULL AS ItemID, CAST(NULL AS VARCHAR(50)) AS ItemName, NULL AS Ammo;
        RETURN;
    END
    IF @IsAdmin = 1
    BEGIN
        SELECT 'ADMIN_HAS_NO_INVENTORY' AS PickUpStatus, NULL AS ItemID, CAST(NULL AS VARCHAR(50)) AS ItemName, NULL AS Ammo;
        RETURN;
    END
    IF @IsOnline = 0
    BEGIN
        SELECT 'NOT_ONLINE' AS PickUpStatus, NULL AS ItemID, CAST(NULL AS VARCHAR(50)) AS ItemName, NULL AS Ammo;
        RETURN;
    END
    IF @TileID IS NULL
    BEGIN
        SELECT 'NO_CURRENT_TILE' AS PickUpStatus, NULL AS ItemID, CAST(NULL AS VARCHAR(50)) AS ItemName, NULL AS Ammo;
        RETURN;
    END
 
    --is there an item on the players tile?
    DECLARE @ItemID INT, @ItemName VARCHAR(50), @ItemAmmo INT;
    SELECT @ItemID = i.ItemID, @ItemName = it.Name, @ItemAmmo = i.Ammo
    FROM dbo.Item i
    JOIN dbo.ItemType it ON it.ItemTypeID = i.ItemTypeID
    WHERE i.TileID = @TileID;
 
    IF @ItemID IS NULL
    BEGIN
        SELECT 'NO_ITEM' AS PickUpStatus, NULL AS ItemID, CAST(NULL AS VARCHAR(50)) AS ItemName, NULL AS Ammo;
        RETURN;
    END
 
    --is the beak already full?
    DECLARE @HeldItemID INT, @HeldName VARCHAR(50);
    SELECT @HeldItemID = i.ItemID, @HeldName = it.Name
    FROM dbo.Item i
    JOIN dbo.ItemType it ON it.ItemTypeID = i.ItemTypeID
    WHERE i.HolderPlayerID = @PlayerID;
 
    IF @HeldItemID IS NOT NULL
    BEGIN
        -- picking up a second pellet gun while holding one adds to its ammo
        IF @HeldName = 'Pellet Gun' AND @ItemName = 'Pellet Gun'
        BEGIN
            UPDATE dbo.Item SET Ammo = ISNULL(Ammo, 0) + ISNULL(@ItemAmmo, 0) WHERE ItemID = @HeldItemID;
            DELETE FROM dbo.Item WHERE ItemID = @ItemID;
 
            SELECT 'AMMO_ADDED' AS PickUpStatus, @HeldItemID AS ItemID, @HeldName AS ItemName,
                   (SELECT Ammo FROM dbo.Item WHERE ItemID = @HeldItemID) AS Ammo;
            RETURN;
        END
 
        SELECT 'INVENTORY_FULL' AS PickUpStatus, NULL AS ItemID, CAST(NULL AS VARCHAR(50)) AS ItemName, NULL AS Ammo;
        RETURN;
    END
 
    --Move the item from the tile into the player's beak
    UPDATE dbo.Item SET TileID = NULL, HolderPlayerID = @PlayerID WHERE ItemID = @ItemID;
 
    SELECT 'SUCCESS' AS PickUpStatus, @ItemID AS ItemID, @ItemName AS ItemName, @ItemAmmo AS Ammo;
END
GO
 
CREATE OR ALTER PROCEDURE dbo.usp_DropItem
    @PlayerID INT
AS
BEGIN
    SET NOCOUNT ON;
 
    DECLARE @IsOnline BIT, @IsAdmin BIT, @TileID INT;
    SELECT @IsOnline = IsOnline, @IsAdmin = IsAdmin, @TileID = CurrentTileID
    FROM dbo.Player
    WHERE PlayerID = @PlayerID;
 
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT 'PLAYER_NOT_FOUND' AS DropStatus, NULL AS ItemID;
        RETURN;
    END
    IF @IsAdmin = 1
    BEGIN
        SELECT 'ADMIN_HAS_NO_INVENTORY' AS DropStatus, NULL AS ItemID;
        RETURN;
    END
    IF @IsOnline = 0
    BEGIN
        SELECT 'NOT_ONLINE' AS DropStatus, NULL AS ItemID;
        RETURN;
    END
 
    DECLARE @ItemID INT;
    SELECT @ItemID = ItemID FROM dbo.Item WHERE HolderPlayerID = @PlayerID;
 
    IF @ItemID IS NULL
    BEGIN
        SELECT 'NOTHING_HELD' AS DropStatus, NULL AS ItemID;
        RETURN;
    END
 
    --standing on their own nest?
    DECLARE @NestID INT;
    SELECT @NestID = NestID FROM dbo.Nest WHERE PlayerID = @PlayerID AND TileID = @TileID;
 
    IF @NestID IS NOT NULL
    BEGIN
        --nest holds up to 9 things, when full dropping does nothing
        IF (SELECT COUNT(*) FROM dbo.Item WHERE NestID = @NestID) >= 9
        BEGIN
            SELECT 'NEST_FULL' AS DropStatus, @ItemID AS ItemID;
            RETURN;
        END
 
        UPDATE dbo.Item SET HolderPlayerID = NULL, NestID = @NestID WHERE ItemID = @ItemID;
        SELECT 'STORED_IN_NEST' AS DropStatus, @ItemID AS ItemID;
        RETURN;
    END
 
    --anywhere else means a dropped item always disappears
    DELETE FROM dbo.Item WHERE ItemID = @ItemID;
    SELECT 'DISCARDED' AS DropStatus, @ItemID AS ItemID;
END
GO
 
CREATE OR ALTER PROCEDURE dbo.usp_TakeFromNest
    @PlayerID INT,
    @ItemID   INT
AS
BEGIN
    SET NOCOUNT ON;
 
    DECLARE @IsOnline BIT, @IsAdmin BIT, @TileID INT;
    SELECT @IsOnline = IsOnline, @IsAdmin = IsAdmin, @TileID = CurrentTileID
    FROM dbo.Player
    WHERE PlayerID = @PlayerID;
 
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT 'PLAYER_NOT_FOUND' AS TakeStatus, NULL AS ItemID;
        RETURN;
    END
    IF @IsAdmin = 1
    BEGIN
        SELECT 'ADMIN_HAS_NO_INVENTORY' AS TakeStatus, NULL AS ItemID;
        RETURN;
    END
    IF @IsOnline = 0
    BEGIN
        SELECT 'NOT_ONLINE' AS TakeStatus, NULL AS ItemID;
        RETURN;
    END
 
    --the nest inventory is only open while standing on the nest
    DECLARE @NestID INT;
    SELECT @NestID = NestID FROM dbo.Nest WHERE PlayerID = @PlayerID AND TileID = @TileID;
 
    IF @NestID IS NULL
    BEGIN
        SELECT 'NOT_ON_NEST' AS TakeStatus, NULL AS ItemID;
        RETURN;
    END
 
    IF NOT EXISTS (SELECT 1 FROM dbo.Item WHERE ItemID = @ItemID AND NestID = @NestID)
    BEGIN
        SELECT 'ITEM_NOT_IN_NEST' AS TakeStatus, NULL AS ItemID;
        RETURN;
    END
 
    IF EXISTS (SELECT 1 FROM dbo.Item WHERE HolderPlayerID = @PlayerID)
    BEGIN
        SELECT 'INVENTORY_FULL' AS TakeStatus, NULL AS ItemID;
        RETURN;
    END
 
    UPDATE dbo.Item SET NestID = NULL, HolderPlayerID = @PlayerID WHERE ItemID = @ItemID;
    SELECT 'SUCCESS' AS TakeStatus, @ItemID AS ItemID;
END
GO
 
--test setup: ZandiGoose (PlayerID 2) online on tile 19 (3,3), where the watermelon is,
--holding his bat. His nest (NestID 1) is on tile 13 and has a shield (ItemID 2) and peas (ItemID 3).
UPDATE dbo.Player SET IsOnline = 1, CurrentTileID = 19 WHERE PlayerID = 2;
 
EXEC dbo.usp_PickUpItem @PlayerID = 2; --INVENTORY_FULL: he is already holding the bat
 
UPDATE dbo.Player SET CurrentTileID = 13 WHERE PlayerID = 2; --walk onto his nest
EXEC dbo.usp_DropItem @PlayerID = 2; --STORED_IN_NEST: the bat goes into the nest
EXEC dbo.usp_DropItem @PlayerID = 2; --NOTHING_HELD
 
UPDATE dbo.Player SET CurrentTileID = 19 WHERE PlayerID = 2;--back to the watermelon
EXEC dbo.usp_PickUpItem @PlayerID = 2; --SUCCESS: he picks up the watermelon
EXEC dbo.usp_PickUpItem @PlayerID = 2; --NO_ITEM: the tile is empty now
 
UPDATE dbo.Player SET CurrentTileID = 12 WHERE PlayerID = 2; --a tile with no nest
EXEC dbo.usp_DropItem @PlayerID = 2; --DISCARDED: the watermelon disappears
EXEC dbo.usp_TakeFromNest @PlayerID = 2, @ItemID = 2; --NOT_ON_NEST
 
UPDATE dbo.Player SET CurrentTileID = 13 WHERE PlayerID = 2; --back on his nest
EXEC dbo.usp_TakeFromNest @PlayerID = 2, @ItemID = 999; --ITEM_NOT_IN_NEST
EXEC dbo.usp_TakeFromNest @PlayerID = 2, @ItemID = 2; --SUCCESS: shield into his beak
EXEC dbo.usp_TakeFromNest @PlayerID = 2, @ItemID = 3; --INVENTORY_FULL: he already has the shield
GO
 
--nest capacity: fill his nest to 9 items, then try to store the shield
DECLARE @n INT = 0;
WHILE @n < 7
BEGIN
    INSERT INTO dbo.Item (ItemTypeID, NestID) VALUES (5, 1); --7 extra peas (nest already holds the bat and the peas)
    SET @n += 1;
END
EXEC dbo.usp_DropItem @PlayerID = 2; --NEST_FULL
DELETE TOP (7) FROM dbo.Item WHERE NestID = 1 AND ItemTypeID = 5 AND ItemID <> 3; --remove the extra peas again
GO
 
--pellet gun ammo stacking: he has a gun with 4 ammo and stands on a tile with another gun (7 ammo)
DELETE FROM dbo.Item WHERE HolderPlayerID = 2; --drop the shield
INSERT INTO dbo.Item (ItemTypeID, HolderPlayerID, Ammo) VALUES (2, 2, 4);
UPDATE dbo.Player SET CurrentTileID = 6 WHERE PlayerID = 2;
EXEC dbo.usp_PickUpItem @PlayerID = 2; --AMMO_ADDED: 4 + 7 = 11 ammo, the gun on tile 6 is used up
 
EXEC dbo.usp_PickUpItem @PlayerID = 999; --PLAYER_NOT_FOUND
EXEC dbo.usp_PickUpItem @PlayerID = 1; --ADMIN_HAS_NO_INVENTORY
EXEC dbo.usp_PickUpItem @PlayerID = 5; --NOT_ONLINE
EXEC dbo.usp_PickUpItem @PlayerID = 6; --NO_CURRENT_TILE
GO
 