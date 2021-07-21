using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Newtonsoft.Json;

namespace DapperSqlParser.TestRepository.Service.DapperExecutor.Extensions
{
    public static class DapperSqlExtensions
    {
        public static IEnumerable<T> QueryJson<T>(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null) where T : class
        {
            var result = cnn.Query<string>(sql, param, transaction, buffered, commandTimeout, commandType).ToList();
            if (!result.Any())
                return default;

            // Concats
            StringBuilder sb = new StringBuilder();
            foreach (string jsonPart in result)
                sb.Append(jsonPart);

            //If needed private fields resolver
            //var settings = new JsonSerializerSettings
            //{
            //    // https://github.com/danielwertheim/jsonnet-contractresolvers
            //    ContractResolver = new PrivateSetterContractResolver()
            //};
            return sb[0] == '['
                ? JsonConvert.DeserializeObject<IEnumerable<T>>(sb.ToString())
                : new[] {JsonConvert.DeserializeObject<T>(sb.ToString())};

            // Using Newtonsoft.Json to de-serialize objects
        }
    }
}