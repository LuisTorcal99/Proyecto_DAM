-- Create SQL Server Login
CREATE LOGIN LuisTorcal
WITH PASSWORD = 'Cambia Contrasena';  

-- Create the database
CREATE DATABASE ProyectoDAM;
GO

-- Use the new database
USE ProyectoDAM;
GO

-- Create user inside the database for the login
CREATE USER LuisTorcal FOR LOGIN LuisTorcal;
GO

-- Grant basic CRUD permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO LuisTorcal;
EXEC sp_addrolemember 'db_datareader', 'LuisTorcal';
EXEC sp_addrolemember 'db_datawriter', 'LuisTorcal';

-- Grant permissions for creating and modifying database objects
GRANT ALTER ON SCHEMA::dbo TO LuisTorcal;
GRANT REFERENCES ON SCHEMA::dbo TO LuisTorcal;
GRANT CREATE TABLE TO LuisTorcal;
GRANT CREATE PROCEDURE TO LuisTorcal;
GRANT CREATE VIEW TO LuisTorcal;
GRANT EXECUTE ON SCHEMA::dbo TO LuisTorcal;
