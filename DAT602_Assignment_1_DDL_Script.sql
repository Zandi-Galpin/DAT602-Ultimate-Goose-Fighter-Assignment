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