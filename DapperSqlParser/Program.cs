using System;
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

            var spModel = await CreateSpDataModelFromOutputParams(paramsList[2]);
            var spNamespace = await CreateSpClient(paramsList.ToArray());
        }

        #region GetSps
        public static async Task<StoredProcedureParameters> GetSpDataAsync(string spName)
        {
            await using var connection = new SqlConnection(ConnectionString);
            connection.Open();
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


        #endregion

        public static async Task<string> CreateSpDataModelFromOutputParams(StoredProcedureParameters parameters)
        {
            if (parameters.OutputParametersDataModels == null) return await Task.FromResult(string.Empty);

            var outputClass = new StringBuilder();
            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}DataModel \n\t{{");

            foreach (var field in parameters.OutputParametersDataModels.Select(p =>
                new string(
                    $"\t\tpublic System.{p.TypeName} " +
                    $"{(p.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{p.ParameterName.Replace("-", "_")}")} {{get; set;}} \n"))
            )
            {
                outputClass.AppendLine(field);

            }

            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }

        public static async Task<string> CreateSpMethodWithInputParameters(StoredProcedureParameters parameters)
        {
            if (parameters == null) return await Task.FromResult(string.Empty);
            var outputClass = new StringBuilder();
            try
            {
                var sortNullableInputs = parameters.InputParametersDataModels!=null? parameters.InputParametersDataModels.ToList().OrderBy(prop => prop.IsNullable):null;
                outputClass.AppendLine(
                    $"\tpublic async  " +
                    $"{(parameters.OutputParametersDataModels == null ? $"Task<{parameters.StoredProcedureInfo.Name}DataModel>" : "Task")}" +//method signature
                    $" {parameters.StoredProcedureInfo.Name}DataModel " +
                    $"(" +
                    $"{(parameters.InputParametersDataModels==null? " " : "")}" +
                    $")" +
                    $"\n\t{{");//method name
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
          

            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }

        public static async Task<string> CreateSpClient(StoredProcedureParameters[] parameters)
        {

            var outputClass = new StringBuilder();
            outputClass.AppendLine($"namespace SpClient \n{{");

            foreach (var spParameter in parameters)
            {

                var modelClass = await CreateSpDataModelFromOutputParams(spParameter);
                outputClass.AppendLine(modelClass);
            }

            outputClass.AppendLine($"\tpublic class SpClient \n\t{{");

            foreach (var spParameter in parameters)
            {
                var spDapperMethod = await CreateSpMethodWithInputParameters(spParameter);
                outputClass.AppendLine(spDapperMethod);
            }

            outputClass.Append("\t}\n");

            outputClass.Append("}");
            return await Task.FromResult(outputClass.ToString());
        }
    }
}
//var path = string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}GeneratedFile\spClient.cs");
