using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace DapperSqlParser.TestRepository.IntegrationTest
{
    public static class DatabaseExtensions
    {
        private static readonly string ProjectPath = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
        private static readonly string ConnectionString = JObject.Parse(File.ReadAllText(Path.Combine(ProjectPath, @"appsettings.json")))["ConnectionStrings"]["UserDb"].ToString();
        private const string DatabaseName = "DapperSPTestDb";
        private const string DatabaseBackupLocation = "C:\\Web\\Microsoft SQL Server\\MSSQL15.SQLEXPRESS\\MSSQL\\Backup\\DapperSPTestDb.bak";


        public static async Task RestoreDatabase()
        {
            await using SqlConnection connection = new SqlConnection(ConnectionString);

            await connection.ExecuteAsync($"ALTER DATABASE[{DatabaseName}] " +
                                          $"SET SINGLE_USER WITH ROLLBACK IMMEDIATE \n" +
                                          $"USE MASTER RESTORE DATABASE [{DatabaseName}] " +
                                          $"FROM DISK = \'{DatabaseBackupLocation}\' WITH REPLACE\nALTER DATABASE [{DatabaseName}] " +
                                          $"SET MULTI_USER", commandType: CommandType.Text);
        }
    }
}