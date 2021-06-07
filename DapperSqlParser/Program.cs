using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using DapperSqlParser.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DapperSqlParser
{
    internal static class Program
    {
        private const string ConnectionString = "Server= .\\SQLExpress;Database=ShopParserDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private static async Task Main(string[] args)
        {

           // var result =await GetSpDataAsync("sp_GetNestedCategoryByParentIdAndCompanyId");
            var SpList = await GetSpListAsync();
            var paramsList = new List<StoredProcedureParameters>();
            foreach (var sp in SpList)
            {
                var spParameter = await GetSpDataAsync(sp.Name);
                paramsList.Add(spParameter);
            }
        }

        public static async Task<StoredProcedureParameters> GetSpDataAsync(string spName)
        {
            await using var connection = new SqlConnection(ConnectionString);

            var values = new { spName };

            var queryResultChunks = await connection.QueryAsync<string>("sp_GetStoredProcedureJsonData", values,
                commandType: CommandType.StoredProcedure);

           return JsonConvert.DeserializeObject<StoredProcedureParameters>(string.Join("", queryResultChunks));
            
        }
        public static async Task<StoredProcedureModel[]> GetSpListAsync()
        {
            await using var connection = new SqlConnection(ConnectionString);
            
            var queryResultChunks = await connection.QueryAsync<string>("sp_GetStoredProcedures",
                commandType: CommandType.StoredProcedure);

            return JsonConvert.DeserializeObject<StoredProcedureModel[]>(string.Join("", queryResultChunks));

        }
    }
}
