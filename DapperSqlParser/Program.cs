using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSqlParser
{
    internal static class Program
    {
        private const string ConnectionString = "Server= .\\SQLExpress;Database=ShopParserDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        private const string NameSpaceName = "SpClient";

        private static async Task Main()
        {
            var spService = new StoredProcedureService(ConnectionString);

            var paramsList = await spService.GenerateModelsListAsync();
            var spNamespace = await CreateSpClient(paramsList.ToArray(), NameSpaceName);
            await WriteGeneratedNamespaceToClientFile(spNamespace);
        }

        //private static async Task Main()
        //{
        //    #region onlyInput(working)

        //    var dapperExecutorIn = new DapperExecutor<sp_UpdateProductStateInput>(ConnectionString);
        //    var spUpdateProductStateClient = new sp_UpdateProductState(dapperExecutorIn);

        //    await spUpdateProductStateClient.Execute(new sp_UpdateProductStateInput() { ProductId = 6, ProductState = 2 });

        //    #endregion

        //    #region onlyOutput(working)

        //    var dapperExecutorOut = new DapperExecutor<EmptyInputParams, sp_GetAllProductsOutput>(ConnectionString);
        //    var spGetAllProductsClient = new sp_GetAllProducts(dapperExecutorOut);

        //    var spGetAllProductsClientResult = await spGetAllProductsClient.Execute();

        //    #endregion

        //    #region InputOutput(working)

        //    var dapperExecutor = new DapperExecutor<sp_CountProductsWithCompanyInput, sp_CountProductsWithCompanyOutput>(ConnectionString);
        //    var spCountProductsWithCompanyClient = new sp_CountProductsWithCompany(dapperExecutor);

        //    var spCountProductsWithCompanyResult = await spCountProductsWithCompanyClient.Execute(new sp_CountProductsWithCompanyInput(){CompanyId = 1});

        //    #endregion
        //}

        public static async Task<string> CreateSpDataModelForOutputParams(StoredProcedureParameters parameters)
        {
            if (parameters.OutputParametersDataModels == null) return await Task.FromResult(string.Empty);

            var outputClass = new StringBuilder();
            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Output \n\t{{");

            foreach (var field in parameters.OutputParametersDataModels.Select(p =>
                new string(
                         $"\t\t{(p.ParameterName == null ? $"" : $"[Newtonsoft.Json.JsonProperty(\"{p.ParameterName}\")]")} " +//If not nullable -> required
                           $"{(p.IsNullable ? "" : "[System.ComponentModel.DataAnnotations.Required()] ")}" +//Json field
                           $"public {p.TypeName} " +//Type name
                           $"{(p.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{p.ParameterName.Replace("-", "_")}")} " +//Param name
                           $"{{get; set;}} \n"))//Getter/setter
            ) outputClass.AppendLine(field);


            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        public static async Task<string> CreateSpDataModelForInputParams(StoredProcedureParameters parameters)
        {
            if (parameters.InputParametersDataModels == null) return await Task.FromResult(string.Empty);

            var outputClass = new StringBuilder();
            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Input \n\t{{");

            foreach (var field in parameters.InputParametersDataModels.Select(p =>
                    new string(
                        $"\t\t{(p.ParameterName == null ? $"" : $"[Newtonsoft.Json.JsonProperty(\"{p.ParameterName.Replace("@", "")}\")]")} " +//If not nullable -> required
                        $"{(p.IsNullable ? "" : "[System.ComponentModel.DataAnnotations.Required()] ")}" +//Json field
                        $"public {p.TypeName} " +//Type name
                        $"{(p.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{p.ParameterName.Replace("-", "_").Replace("@", "").FirstCharToUpper()}")} " +//Param name
                        $"{{get; set;}} \n"))//Getter/setter
            ) outputClass.AppendLine(field);


            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        public static async Task<string> CreateSpClientClass(StoredProcedureParameters parameters)
        {

            var outputClass = new StringBuilder();

            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name} \n\t{{");//Class name

            if (parameters.InputParametersDataModels != null && parameters.OutputParametersDataModels != null)//IF inputs and outputs
            {
                outputClass.AppendLine(
                    $"\t\tprivate readonly IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input, {parameters.StoredProcedureInfo.Name}Output> _dapperExecutor;");//DapperExecutor prop

                outputClass.AppendLine(
                    $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input, {parameters.StoredProcedureInfo.Name}Output> dapperExecutor)\n\t\t{{" +//Ctor 
                    $"\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                    $"\n\t\t}}");

                outputClass.AppendLine(
                    $"\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<{parameters.StoredProcedureInfo.Name}Output>> Execute({parameters.StoredProcedureInfo.Name}Input request)\n\t\t{{" +//Execute method
                    $"\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"{parameters.StoredProcedureInfo.Name}\", request);" +
                    $"\n\t\t}}");
            }
            else if (parameters.InputParametersDataModels != null && parameters.OutputParametersDataModels == null)//IF inputs
            {
                outputClass.AppendLine(
                    $"\t\tprivate readonly IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input> _dapperExecutor;");//DapperExecutor prop

                outputClass.AppendLine(
                    $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<{parameters.StoredProcedureInfo.Name}Input>dapperExecutor)\n\t\t{{" +//Ctor 
                    $"\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                    $"\n\t\t}}");

                outputClass.AppendLine(
                    $"\t\tpublic System.Threading.Tasks.Task Execute({parameters.StoredProcedureInfo.Name}Input request)\n\t\t{{" +//Execute method
                    $"\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"{parameters.StoredProcedureInfo.Name}\", request);" +
                    $"\n\t\t}}");
            }
            else//IF outputs
            {
                outputClass.AppendLine(
                    $"\t\tprivate readonly IDapperExecutor<EmptyInputParams, {parameters.StoredProcedureInfo.Name}Output> _dapperExecutor;");//DapperExecutor prop

                outputClass.AppendLine(
                    $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<EmptyInputParams, {parameters.StoredProcedureInfo.Name}Output> dapperExecutor)\n\t\t{{" +//Ctor 
                    $"\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                    $"\n\t\t}}");

                outputClass.AppendLine(
                    $"\t\tpublic System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<{parameters.StoredProcedureInfo.Name}Output>> Execute()\n\t\t{{" +//Execute method
                    $"\n\t\t\treturn _dapperExecutor.ExecuteAsync(\"{parameters.StoredProcedureInfo.Name}\");" +
                    $"\n\t\t}}");
            }


            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        public static async Task<string> CreateSpClient(IEnumerable<StoredProcedureParameters> parameters, string namespaceName)
        {

            var outputNamespace = new StringBuilder();
            outputNamespace.AppendLine($"namespace {namespaceName} \n{{");

            foreach (var spParameter in parameters)
            {
                outputNamespace.AppendLine($"\n\t#region {spParameter.StoredProcedureInfo.Name}");//Wrapping every sp into region

                var outputModelClass = await CreateSpDataModelForOutputParams(spParameter);
                var inputModelClass = await CreateSpDataModelForInputParams(spParameter);
                var clientClass = await CreateSpClientClass(spParameter);

                outputNamespace.AppendLine(outputModelClass);
                outputNamespace.AppendLine(inputModelClass);
                outputNamespace.AppendLine(clientClass);

                outputNamespace.AppendLine("\t#endregion");
            }

            outputNamespace.Append("}");
            return await Task.FromResult(outputNamespace.ToString());
        }
        public static async Task WriteGeneratedNamespaceToClientFile(string namespaceString)
        {
            // This will get the current PROJECT directory
            var projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            var filePath = Path.Combine(projectPath ?? throw new InvalidOperationException(), @"GeneratedFile\spClient.cs");

            await File.WriteAllTextAsync(filePath, namespaceString);
        }

    }
}