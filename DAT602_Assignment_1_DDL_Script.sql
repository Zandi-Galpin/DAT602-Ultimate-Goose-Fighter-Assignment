-- DAT602 Assignment 1, Ultimate Goose Fighter
-- Zandi Galpin

-- one TSQL procedure that creates every table, constraint,
-- secondary index, and a set of test data covering all CRUD situations.

--Rebuilds the database from scratch each time.
-- SINGLE_USER + ROLLBACK IMMEDIATE gets rid of any other open connections

USE master;
GO

IF DB_ID(N'GooseFighterDB') IS NOT NULL
BEGIN
    ALTER DATABASE GooseFighterDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE GooseFighterDB;
END
GO

CREATE DATABASE GooseFighterDB;
GO

USE GooseFighterDB;
GO

CREATE OR ALTER PROCEDURE dbo.usp_CreateGooseFighterDatabase
AS

BEGIN
    SET NOCOUNT ON;

    -- Drop existing tables (child tables first)
    IF OBJECT_ID('dbo.ChatMessage', 'U') IS NOT NULL DROP TABLE dbo.ChatMessage;
    IF OBJECT_ID('dbo.Item', 'U')        IS NOT NULL DROP TABLE dbo.Item;
    IF OBJECT_ID('dbo.NPC', 'U')         IS NOT NULL DROP TABLE dbo.NPC;
    IF OBJECT_ID('dbo.ItemType', 'U')    IS NOT NULL DROP TABLE dbo.ItemType;
    IF OBJECT_ID('dbo.Nest', 'U')        IS NOT NULL DROP TABLE dbo.Nest;
    IF OBJECT_ID('dbo.Player', 'U')      IS NOT NULL DROP TABLE dbo.Player;
    IF OBJECT_ID('dbo.Tile', 'U')        IS NOT NULL DROP TABLE dbo.Tile;


    --TILE: the game board. No dependencies (made first)
    CREATE TABLE dbo.Tile (
        TileID      INT IDENTITY(1,1) NOT NULL,
        XPosition   INT NOT NULL,
        YPosition   INT NOT NULL,
        IsOccupied  BIT NOT NULL DEFAULT (0),
        CONSTRAINT PK_Tile PRIMARY KEY CLUSTERED (TileID),
        CONSTRAINT UQ_Tile_Position UNIQUE (XPosition, YPosition)
    );

    --PLAYER: accounts and live player state.
    CREATE TABLE dbo.Player (
        PlayerID             INT IDENTITY(1,1) NOT NULL,
        Email                VARCHAR(255) NOT NULL,
        Username             VARCHAR(100)  NOT NULL,
        Password             VARCHAR(100) NOT NULL,
        Score                INT NOT NULL DEFAULT (0),
        Health               INT NOT NULL DEFAULT (4000),
        IsAdmin              BIT NOT NULL DEFAULT (0),
        IsOnline             BIT NOT NULL DEFAULT (0),
        FailedLoginAttempts  INT NOT NULL DEFAULT (0),
        IsLockedOut          BIT NOT NULL DEFAULT (0),
        CurrentTileID        INT NULL,
        IsInvincible         BIT NOT NULL DEFAULT (0),
        InvincibleUntil      DATETIME NULL,
        CreatedDate          DATETIME NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT PK_Player PRIMARY KEY CLUSTERED (PlayerID),
        CONSTRAINT UQ_Player_Email UNIQUE (Email),
        CONSTRAINT UQ_Player_Username UNIQUE (Username),
        CONSTRAINT FK_Player_Tile FOREIGN KEY (CurrentTileID)
            REFERENCES dbo.Tile (TileID),
        CONSTRAINT CK_Player_FailedLoginAttempts
            CHECK (FailedLoginAttempts BETWEEN 0 AND 5),
        CONSTRAINT CK_Player_Health
            CHECK (Health BETWEEN 0 AND 4000)
    );

    --Secondary index: because the list for who is online is queried very very often
    CREATE NONCLUSTERED INDEX IX_Player_IsOnline ON dbo.Player (IsOnline);


    --NEST: one per player, sits on the tile they spawn in on.
    CREATE TABLE dbo.Nest (
        NestID    INT IDENTITY(1,1) NOT NULL,
        PlayerID  INT NOT NULL,
        TileID    INT NOT NULL,
        CONSTRAINT PK_Nest PRIMARY KEY CLUSTERED (NestID),
        CONSTRAINT UQ_Nest_Player UNIQUE (PlayerID),   --1 nest per player
        CONSTRAINT UQ_Nest_Tile UNIQUE (TileID),       --1 nest per tile
        CONSTRAINT FK_Nest_Player FOREIGN KEY (PlayerID)
            REFERENCES dbo.Player (PlayerID) ON DELETE CASCADE,
        CONSTRAINT FK_Nest_Tile FOREIGN KEY (TileID)
            REFERENCES dbo.Tile (TileID)
    );

    --ITEMTYPE: lookup table for holdable/consumable items.
    CREATE TABLE dbo.ItemType (
        ItemTypeID         INT IDENTITY(1,1) NOT NULL,
        Name               VARCHAR(50) NOT NULL,
        Category           VARCHAR(20) NOT NULL,
        EffectDescription  VARCHAR(200) NULL,
        SpawnRate          DECIMAL(5,2) NULL,
        CONSTRAINT PK_ItemType PRIMARY KEY CLUSTERED (ItemTypeID),
        CONSTRAINT UQ_ItemType_Name UNIQUE (Name),
        CONSTRAINT CK_ItemType_Category
            CHECK (Category IN ('Holdable', 'Consumable')),
        CONSTRAINT CK_ItemType_SpawnRate
            CHECK (SpawnRate IS NULL OR SpawnRate BETWEEN 0 AND 100)
    );

    --ITEM: one row per item. Exactly one of TileID/HolderPlayerID/NestID is non null at once.
    CREATE TABLE dbo.Item (
        ItemID          INT IDENTITY(1,1) NOT NULL,
        ItemTypeID      INT NOT NULL,
        TileID          INT NULL,
        HolderPlayerID  INT NULL,
        NestID          INT NULL,
        Ammo            INT NULL,
        CONSTRAINT PK_Item PRIMARY KEY CLUSTERED (ItemID),
        CONSTRAINT FK_Item_ItemType FOREIGN KEY (ItemTypeID)
            REFERENCES dbo.ItemType (ItemTypeID),
        CONSTRAINT FK_Item_Tile FOREIGN KEY (TileID)
            REFERENCES dbo.Tile (TileID),
        CONSTRAINT FK_Item_Player FOREIGN KEY (HolderPlayerID)
            REFERENCES dbo.Player (PlayerID),
        CONSTRAINT FK_Item_Nest FOREIGN KEY (NestID)
            REFERENCES dbo.Nest (NestID),
        CONSTRAINT CK_Item_Ammo CHECK (Ammo IS NULL OR Ammo >= 0),
        CONSTRAINT CK_Item_OneLocation CHECK (
            (CASE WHEN TileID IS NOT NULL THEN 1 ELSE 0 END
           + CASE WHEN HolderPlayerID IS NOT NULL THEN 1 ELSE 0 END
           + CASE WHEN NestID IS NOT NULL THEN 1 ELSE 0 END) = 1
        )
    );

    CREATE NONCLUSTERED INDEX IX_Item_TileID ON dbo.Item (TileID);
    CREATE NONCLUSTERED INDEX IX_Item_HolderPlayerID ON dbo.Item (HolderPlayerID);
    CREATE NONCLUSTERED INDEX IX_Item_NestID ON dbo.Item (NestID);

    --NPC (grey goose) table
    CREATE TABLE dbo.NPC (
        NPCID        INT IDENTITY(1,1) NOT NULL,
        TileID       INT NOT NULL,
        Health       INT NOT NULL DEFAULT (1000),
        MaxHealth    INT NOT NULL DEFAULT (1000),
        IsAlive      BIT NOT NULL DEFAULT (1),
        RespawnTime  DATETIME NULL,
        CONSTRAINT PK_NPC PRIMARY KEY CLUSTERED (NPCID),
        CONSTRAINT FK_NPC_Tile FOREIGN KEY (TileID)
            REFERENCES dbo.Tile (TileID),
        CONSTRAINT CK_NPC_Health CHECK (Health BETWEEN 0 AND MaxHealth)
    );

    --CHATMESSAGE: global broadcast chat
    CREATE TABLE dbo.ChatMessage (
        MessageID       INT IDENTITY(1,1) NOT NULL,
        SenderPlayerID  INT NOT NULL,
        MessageText     VARCHAR(500) NOT NULL,
        SentDate        DATETIME NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT PK_ChatMessage PRIMARY KEY CLUSTERED (MessageID),
        CONSTRAINT FK_ChatMessage_Player FOREIGN KEY (SenderPlayerID)
            REFERENCES dbo.Player (PlayerID) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_ChatMessage_SentDate ON dbo.ChatMessage (SentDate);


    --TEST DATA: covers every CRUD situation from the CRUD table:
    --login/lockout, registration, tile layout, item placement,
    --movement, scoring, inventory, NPC/item movement, chat
    -- Tiles: a 5x5 board (25 tiles), IDs assigned 1-25 in x-then-y order
    DECLARE @x INT = 0, @y INT;
    WHILE @x < 5
    BEGIN
        SET @y = 0;
        WHILE @y < 5
        BEGIN
            INSERT INTO dbo.Tile (XPosition, YPosition) VALUES (@x, @y);
            SET @y += 1;
        END
        SET @x += 1;
    END

    --Players: has admin, online player, offline player,
    --locked-out player, and an admin created player who has
    -- never logged in (no nest and no CurrentTileID yet)
    INSERT INTO dbo.Player (Email, Username, Password, IsAdmin, IsOnline)
    VALUES ('admin@goosefighter.com', 'AdminGoose', 'AdminPass123!', 1, 0);

    INSERT INTO dbo.Player (Email, Username, Password, Score, Health, IsOnline, CurrentTileID)
    VALUES ('zandi@example.com', 'ZandiGoose', 'Password123', 3, 3100, 1, 13);

    INSERT INTO dbo.Player (Email, Username, Password, Score, Health, IsOnline, CurrentTileID)
    VALUES ('mcplayer@example.com', 'McPlayerTwoington', 'Passw0rd!', 5, 4000, 0, 7);

    INSERT INTO dbo.Player (Email, Username, Password, FailedLoginAttempts, IsLockedOut)
    VALUES ('lockedout@example.com', 'LockedGoose', 'wrongpass', 5, 1);

    INSERT INTO dbo.Player (Email, Username, Password)
    VALUES ('newplayer@example.com', 'BranfNewGoose', 'TempPass1');

    --nests for the two players who have spawned in
    INSERT INTO dbo.Nest (PlayerID, TileID) VALUES (2, 13); -- ZandiGoose
    INSERT INTO dbo.Nest (PlayerID, TileID) VALUES (3, 7);  -- McPlayerTwoington

    --item types: all 4 holdable + 4 consumable
    INSERT INTO dbo.ItemType (Name, Category, EffectDescription, SpawnRate) VALUES
        ('Baseball Bat', 'Holdable',   '+50% attack damage',                         15.00),
        ('Pellet Gun',   'Holdable',   '-50% attack damage, ranged, 3-10 ammo',      10.00),
        ('Shield',       'Holdable',   '-50% incoming damage, -25% outgoing damage', 12.00),
        ('Grenade',      'Holdable',   'Thrown 3 tiles, 500% damage in 3x3 area',     5.00),
        ('Peas',         'Consumable', 'Regain 200 health',                          30.00),
        ('Banana',       'Consumable', 'Regain 400 health',                          15.00),
        ('Grapes',       'Consumable', 'Invincibility for 5 seconds',                 5.00),
        ('Watermelon',   'Consumable', 'Regain 1000 health',                          5.00);

    --Items in all location states
    INSERT INTO dbo.Item (ItemTypeID, HolderPlayerID) VALUES (1, 2);        --bat held by player
    INSERT INTO dbo.Item (ItemTypeID, NestID)         VALUES (3, 1);        --shield stored in player's nest
    INSERT INTO dbo.Item (ItemTypeID, NestID)         VALUES (5, 1);        --peas stored in player's nest
    INSERT INTO dbo.Item (ItemTypeID, TileID)         VALUES (8, 19);       --watermelon lying on a tile
    INSERT INTO dbo.Item (ItemTypeID, TileID, Ammo)   VALUES (2, 6, 7);     --pellet gun on a tile with ammo
    
    --NPC
    INSERT INTO dbo.NPC (TileID, Health, MaxHealth, IsAlive) VALUES (11, 1000, 1000, 1);

    --chat messages
    INSERT INTO dbo.ChatMessage (SenderPlayerID, MessageText)
    VALUES (2, 'yoooo whats up guys????');
    INSERT INTO dbo.ChatMessage (SenderPlayerID, MessageText)
    VALUES (3, 'WATCH OUT HES GOT A PELLET GUN');

END
GO


--procedure to build the database and populate test data
EXEC dbo.usp_CreateGooseFighterDatabase;
GO




