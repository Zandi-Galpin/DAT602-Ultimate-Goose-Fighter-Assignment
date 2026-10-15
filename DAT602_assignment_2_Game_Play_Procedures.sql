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

