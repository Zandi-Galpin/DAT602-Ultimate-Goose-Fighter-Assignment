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
        Email                VARCHAR(100) NOT NULL,
        Username             VARCHAR(50)  NOT NULL,
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

END
GO

--procedure to build the database and populate test data
EXEC dbo.usp_CreateGooseFighterDatabase;
GO

