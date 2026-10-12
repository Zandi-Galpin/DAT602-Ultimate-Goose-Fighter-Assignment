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