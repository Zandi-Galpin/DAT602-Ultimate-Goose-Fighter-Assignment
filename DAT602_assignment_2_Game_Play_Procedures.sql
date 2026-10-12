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