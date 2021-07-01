using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;

namespace DapperSqlParser.TestRepository.IntegrationTest
{
    public static class DatabaseExtensions
    {
        private static string ProjectPath => Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
        private static string ConnectionString => JObject.Parse(File.ReadAllText(Path.Combine(ProjectPath, @"appsettings.json")))["ConnectionStrings"]["UserDb"].ToString();
        private static string DatabaseName => "DapperSPTestDb";
        private static string DatabaseBackupLocation =>
            "C:\\Web\\Microsoft SQL Server\\MSSQL15.SQLEXPRESS\\MSSQL\\Backup\\DapperSPTestDb.bak";


        public static async Task RestoreDatabase()
        {
            await using var connection = new SqlConnection(ConnectionString);

            await connection.ExecuteAsync($"" +
                                          $"ALTER DATABASE[{DatabaseName}] " +
                                          $"SET SINGLE_USER WITH ROLLBACK IMMEDIATE \n" +
                                          $"USE MASTER RESTORE DATABASE [{DatabaseName}] " +
                                          $"FROM DISK = \'{DatabaseBackupLocation}\' WITH REPLACE\nALTER DATABASE [{DatabaseName}] " +
                                          $"SET MULTI_USER", commandType: CommandType.Text);
        }
    }
}