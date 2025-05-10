using Microsoft.Data.SqlClient;

namespace RestAPI
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseSetup(IConfiguration configuration)
        {
            var masterConnectionString = configuration.GetConnectionString("MasterConnection");

            using var connection = new SqlConnection(masterConnectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"
            IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'LuisTorcal')
            BEGIN
                CREATE LOGIN LuisTorcal WITH PASSWORD = 'e.d_fwm2()~37hz?+LBT4V';
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

            command.CommandText = @"
            IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'LuisTorcal')
            BEGIN
                CREATE USER LuisTorcal FOR LOGIN LuisTorcal;
            END";
            command.ExecuteNonQuery();

            command.CommandText = "EXEC sp_addrolemember 'db_datareader', 'LuisTorcal';";
            command.ExecuteNonQuery();

            command.CommandText = "EXEC sp_addrolemember 'db_datawriter', 'LuisTorcal';";
            command.ExecuteNonQuery();

            command.CommandText = "GRANT ALTER ON SCHEMA::dbo TO LuisTorcal;";
            command.ExecuteNonQuery();

            command.CommandText = "GRANT REFERENCES ON SCHEMA::dbo TO LuisTorcal;";
            command.ExecuteNonQuery();

            command.CommandText = "GRANT CREATE TABLE TO LuisTorcal;";
            command.ExecuteNonQuery();

            command.CommandText = "GRANT CREATE PROCEDURE TO LuisTorcal;";
            command.ExecuteNonQuery();

            command.CommandText = "GRANT CREATE VIEW TO LuisTorcal;";
            command.ExecuteNonQuery();
        }
    }
}
