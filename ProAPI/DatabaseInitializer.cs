using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RestAPI
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseSetup(IConfiguration configuration)
        {
            var masterConnectionString = configuration.GetConnectionString("MasterConnection");

            var userName = configuration["UserLuisTorcal:UserName"];
            var password = configuration["UserLuisTorcal:Password"];

            using var connection = new SqlConnection(masterConnectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = $@"
            IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '{userName}')
            BEGIN
                CREATE LOGIN [{userName}] WITH PASSWORD = '{password}';
            END";
            command.ExecuteNonQuery();

            command.CommandText = @"
            IF DB_ID('ProyectoDAM') IS NULL
            BEGIN
                CREATE DATABASE ProyectoDAM;
            END";
            command.ExecuteNonQuery();

            command.CommandText = "USE ProyectoDAM;";
            command.ExecuteNonQuery();

            command.CommandText = $@"
            IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '{userName}')
            BEGIN
                CREATE USER [{userName}] FOR LOGIN [{userName}];
            END";
            command.ExecuteNonQuery();

            command.CommandText = $@"EXEC sp_addrolemember 'db_datareader', '{userName}';";
            command.ExecuteNonQuery();

            command.CommandText = $@"EXEC sp_addrolemember 'db_datawriter', '{userName}';";
            command.ExecuteNonQuery();

            command.CommandText = $@"GRANT ALTER ON SCHEMA::dbo TO [{userName}];";
            command.ExecuteNonQuery();

            command.CommandText = $@"GRANT REFERENCES ON SCHEMA::dbo TO [{userName}];";
            command.ExecuteNonQuery();

            command.CommandText = $@"GRANT CREATE TABLE TO [{userName}];";
            command.ExecuteNonQuery();

            command.CommandText = $@"GRANT CREATE PROCEDURE TO [{userName}];";
            command.ExecuteNonQuery();

            command.CommandText = $@"GRANT CREATE VIEW TO [{userName}];";
            command.ExecuteNonQuery();
        }
    }
}
