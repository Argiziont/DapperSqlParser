using Dapper;
using DapperSqlParser.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var spModel = await CreateSpDataModelFromOutputParams(paramsList[2].OutputParametersDataModels);
            var spNamespace = await CreateSpClient(paramsList.ToArray());
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

        public static async Task<string> CreateSpDataModelFromOutputParams(OutputParametersDataModel[] parameters)
        {
            if (parameters == null) return await Task.FromResult(string.Empty);
            var outputClass = new StringBuilder();
            outputClass.AppendLine($"\tpublic class {parameters.First().Name}DataModel \n\t{{");
            foreach (var field in parameters.Select(p =>
                new string(
                    $"\t\tpublic System.{p.TypeName} " +
                    $"{(p.ParameterName == null ? $"{parameters.First().Name}Result" : $"{p.ParameterName.Replace("-", "_")}")} {{get; set;}} \n"))
            )
            {
                outputClass.AppendLine(field);

            }

            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        public static async Task<string> CreateSpClient(StoredProcedureParameters[] parameters)
        {

            var outputClass = new StringBuilder();
            outputClass.AppendLine($"namespace SpClient \n{{");

            foreach (var p in parameters)
            {
                var modelClass = await CreateSpDataModelFromOutputParams(p.OutputParametersDataModels);
                outputClass.AppendLine(modelClass);
            }

            outputClass.Append("}");
            return await Task.FromResult(outputClass.ToString());
        }
    }
}
//var path = string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}GeneratedFile\spClient.cs");
