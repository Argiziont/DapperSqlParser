using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using DapperSqlParser.Models;
using Newtonsoft.Json;

namespace DapperSqlParser.Services
{
    public class StoredProcedureService
    {
        private readonly string _connectionString;

        public StoredProcedureService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<StoredProcedureParameters> GetSpDataAsync(string spName)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var values = new { spName };

            var queryResultChunks = await connection.QueryAsync<string>("sp_GetStoredProcedureJsonData", values,
                commandType: CommandType.StoredProcedure);

            return JsonConvert.DeserializeObject<StoredProcedureParameters>(string.Join("", queryResultChunks));

        }
        public async Task<StoredProcedureModel[]> GetSpListAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            var queryResultChunks = await connection.QueryAsync<string>("sp_GetStoredProcedures",
                commandType: CommandType.StoredProcedure);

            return JsonConvert.DeserializeObject<StoredProcedureModel[]>(string.Join("", queryResultChunks));

        }
    }
}