--Test procedures called by the C# prototype's DAO classes.
--Run DAT602_CreateDatabase.sql first before this to create and set up the database.

USE GooseFighterDB;
GO

--called by LoginDao.AttemptLogin()
CREATE OR ALTER PROCEDURE dbo.usp_TestLogin
    @Username VARCHAR(100),
    @Password VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PlayerID, IsAdmin, IsLockedOut
    FROM dbo.Player
    WHERE Username = @Username AND Password = @Password;
END
GO

-- Called by GamePlayDao.MovePlayer()
CREATE OR ALTER PROCEDURE dbo.usp_TestMovePlayer
    @PlayerID INT,
    @NewTileID INT
AS
BEGIN
    UPDATE dbo.Player
    SET CurrentTileID = @NewTileID
    WHERE PlayerID = @PlayerID;
END
GO

--Called by AdminDao.AddPlayer()
CREATE OR ALTER PROCEDURE dbo.usp_TestAddPlayer
    @Email VARCHAR(255),
    @Username VARCHAR(100),
    @Password VARCHAR(100),
    @IsAdmin BIT
AS
BEGIN

    INSERT INTO dbo.Player (Email, Username, Password, IsAdmin)
    VALUES (@Email, @Username, @Password, @IsAdmin);
END
GO