using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.Services;
using DapperSqlParser.Services.Exceptions;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using ShopParserApi.Services.GeneratedClientFile;
using static DapperSqlParser.Services.TemplateService.TemplateNamingConstants;

namespace DapperSqlParser
{
    internal static class Program
    {
        private const string ConnectionString =
            "Server= .\\SQLExpress;Database=DapperSPTestDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private const string NameSpaceName = "ShopParserApi.Services.GeneratedClientFile";

        private static async Task Main(string[] args)
        {
            if (!args.Any())
                Console.WriteLine("Specify stored procedures for parsing: \n" +
                                  "\t-all :for all procedures\n" +
                                  "\t-mod [sp1],[sp2],[sp3],[...] :for procedures with given names");

            var spService = new StoredProcedureService(ConnectionString);
            List<StoredProcedureParameters> paramsList;

            if (args.Contains("-all"))
                paramsList = await spService.GenerateModelsListAsync();
            else if (args.Contains("-mod"))
                paramsList = await spService.GenerateModelsListAsync(args[1].Split(','));
            else
                return;
            var spNamespace = await CreateSpClient(paramsList.ToArray(), NameSpaceName);
            await WriteGeneratedNamespaceToClientFile(spNamespace);
        }

        public static async Task<string> CreateSpDataModelForOutputParams(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parameters.OutputParametersDataModels == null) return await Task.FromResult(string.Empty);

            var outputClass = new StringBuilder();

            if (parameters.OutputParametersDataModels.First().ParameterName != null && Guid.TryParse(
                parameters.OutputParametersDataModels.First().ParameterName.Replace("JSON_", ""),
                out _))
            {
                var jsonSchemaStartIndex =
                    parameters.StoredProcedureText.Definition.IndexOf(OutputSchemeStartKeyWordSnippet,
                        StringComparison.Ordinal);
                var jsonSchemaEndIndex =
                    parameters.StoredProcedureText.Definition.IndexOf(OutputSchemeEndKeyWordSnippet,
                        StringComparison.Ordinal);

                if (jsonSchemaStartIndex == -1 && jsonSchemaEndIndex == -1)
                    throw new NullModelException();

                jsonSchemaStartIndex += OutputSchemeStartKeyWordSnippet.Length;
                jsonSchemaEndIndex -= jsonSchemaStartIndex;

                var jsonSchemaFromSp =
                    parameters.StoredProcedureText.Definition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

                //Schema parsing from JSON to csSharp
                var schema = await JsonSchema.FromJsonAsync(jsonSchemaFromSp);
                var generator = new CSharpGenerator(schema);

                var trimStart = "#pragma warning disable // Disable all warnings";

                var generatedClasses = generator.GenerateFile();

                var nameSpaceBracketIndex = generatedClasses.IndexOf(trimStart, StringComparison.Ordinal) +
                                            trimStart.Length + 2;
                outputClass = new StringBuilder(generatedClasses.Substring(nameSpaceBracketIndex,
                    generatedClasses.Length - 2 - nameSpaceBracketIndex));


                return await Task.FromResult(outputClass.ToString());
            }
            //Sp returns fields with data

            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Output \n\t{{");

            foreach (var field in parameters.OutputParametersDataModels) //Getter/setter
                AppendOutputParameterPropertyField(parameters, outputClass, field);

            outputClass.Append("\t}\n");

            return await Task.FromResult(outputClass.ToString());
        }

        public static async Task<string> CreateSpDataModelForInputParams(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parameters.InputParametersDataModels == null) return await Task.FromResult(string.Empty);

            var inputClass = new StringBuilder();


            var jsonSchemaStartIndex =
                parameters.StoredProcedureText.Definition.IndexOf(InputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            var jsonSchemaEndIndex =
                parameters.StoredProcedureText.Definition.IndexOf(InputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex != -1 && jsonSchemaEndIndex != -1)
            {
                jsonSchemaStartIndex += InputSchemeStartKeyWordSnippet.Length;
                jsonSchemaEndIndex -= jsonSchemaStartIndex;

                var jsonSchemaFromSp =
                    parameters.StoredProcedureText.Definition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

                //Schema parsing from JSON to csSharp
                var schema = await JsonSchema.FromJsonAsync(jsonSchemaFromSp);
                var generator = new CSharpGenerator(schema);

                var trimStart = "#pragma warning disable // Disable all warnings";

                var generatedClasses = generator.GenerateFile();

                var nameSpaceBracketIndex =
                    generatedClasses.IndexOf(trimStart, StringComparison.Ordinal) + trimStart.Length + 2;
                inputClass =
                    new StringBuilder(
                        $"\t[JsonWrapper(\"{parameters.InputParametersDataModels.First().ParameterName}\")]\n" +
                        generatedClasses.Substring(nameSpaceBracketIndex,
                            generatedClasses.Length - 2 - nameSpaceBracketIndex));

                return await Task.FromResult(inputClass.ToString());
            }


            inputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Input \n\t{{");

            foreach (var field in parameters.InputParametersDataModels) //Getter/setter
                AppendInputParameterPropertyField(parameters, inputClass, field);


            inputClass.Append("\t}\n");
            return await Task.FromResult(inputClass.ToString());
        }

        public static async Task<string> CreateSpClientClass(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var outputClass = new StringBuilder();


            #region CheckSpForJsonOutput

            var spReturnJsonFlag = parameters.OutputParametersDataModels?.First().ParameterName != null &&
                                   Guid.TryParse(
                                       parameters.OutputParametersDataModels.First().ParameterName.Replace("JSON_", ""),
                                       out _);

            #endregion

            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name} \n\t{{"); //Class name

            AppendIDapperExecutorField(parameters, outputClass);
            AppendClientConstructor(parameters, outputClass);
            AppendExecutorMethod(parameters, outputClass, spReturnJsonFlag);

            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }

        public static async Task<string> CreateSpClient(IEnumerable<StoredProcedureParameters> parameters,
            string namespaceName)
        {
            var outputNamespace = new StringBuilder();
            outputNamespace.AppendLine($"namespace {namespaceName} \n{{");

            foreach (var spParameter in parameters)
            {
                outputNamespace.AppendLine(
                    $"\n\t#region {spParameter.StoredProcedureInfo.Name}"); //Wrapping every sp into region

                try
                {
                    var outputModelClass = await CreateSpDataModelForOutputParams(spParameter);
                    var inputModelClass = await CreateSpDataModelForInputParams(spParameter);
                    var clientClass = await CreateSpClientClass(spParameter);

                    outputNamespace.AppendLine(outputModelClass);
                    outputNamespace.AppendLine(inputModelClass);
                    outputNamespace.AppendLine(clientClass);
                }
                catch (NullModelException)
                {
                    outputNamespace.AppendLine(
                        $"//Model for {spParameter.StoredProcedureInfo.Name} was not found, could not parse this Stored Procedure!");
                }


                outputNamespace.AppendLine("\t#endregion");
            }

            outputNamespace.Append("}");
            return await Task.FromResult(outputNamespace.ToString());
        }

        public static async Task WriteGeneratedNamespaceToClientFile(string namespaceString)
        {
            // This will get the current PROJECT directory
            var projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            var filePath = Path.Combine(projectPath ?? throw new InvalidOperationException(),
                @"GeneratedFile\spClient.cs");

            await File.WriteAllTextAsync(filePath, namespaceString);
        }

        private static void AppendInputParameterPropertyField(StoredProcedureParameters parameters,
            StringBuilder inputClass,
            InputParametersDataModel field)
        {
            inputClass.AppendLine(new string(
                $"\t\t{(field.ParameterName == null ? "" : $"[Newtonsoft.Json.JsonProperty(\"{field.ParameterName.Replace("@", "")}\")]")} " + //If not nullable -> required
                $"{(field.IsNullable ? "" : "[System.ComponentModel.DataAnnotations.Required()] ")}" + //Json field
                $"public {field.TypeName} " + //Type name
                $"{(field.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{field.ParameterName.Replace("-", "_").Replace("@", "").FirstCharToUpper()}")} " + //Param name
                "{get; set;} \n"));
        }

        private static void AppendClientConstructor(StoredProcedureParameters parameters, StringBuilder outputClass)
        {
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (outputClass == null) throw new ArgumentNullException(nameof(outputClass));


            outputClass.AppendLine(
                $"\t\tpublic {parameters.StoredProcedureInfo.Name}(IDapperExecutor<{(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input" : "EmptyInputParams")}" +
                $"{(parameters.OutputParametersDataModels != null ? $", {parameters.StoredProcedureInfo.Name}Output" : "")}> dapperExecutor)\n\t\t{{" + //Ctor 
                "\n\t\t\tthis._dapperExecutor = dapperExecutor;" +
                "\n\t\t}");
        }

        private static void AppendExecutorMethod(StoredProcedureParameters parameters, StringBuilder outputClass,
            bool spReturnJsonFlag)
        {
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (outputClass == null) throw new ArgumentNullException(nameof(outputClass));

            outputClass.AppendLine(
                $"\t\tpublic System.Threading.Tasks.Task{(parameters.OutputParametersDataModels != null ? $"<System.Collections.Generic.IEnumerable<{parameters.StoredProcedureInfo.Name}Output>>" : "")} " +
                $"Execute({(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input request" : "")} )\n\t\t{{" + //Execute method
                $"\n\t\t\treturn _dapperExecutor.{(spReturnJsonFlag ? "ExecuteJsonAsync" : "ExecuteAsync")}(\"{parameters.StoredProcedureInfo.Name}\"{(parameters.InputParametersDataModels != null ? ", request" : "")});" +
                "\n\t\t}"); //If input and output
        }

        private static void AppendIDapperExecutorField(StoredProcedureParameters parameters, StringBuilder outputClass)
        {
            if (parameters.InputParametersDataModels == null && parameters.OutputParametersDataModels == null)
                throw new ArgumentNullException(nameof(parameters.InputParametersDataModels) + " " +
                                                nameof(parameters.OutputParametersDataModels));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (outputClass == null) throw new ArgumentNullException(nameof(outputClass));

            outputClass.AppendLine(
                "\t\tprivate readonly " +
                $"IDapperExecutor<{(parameters.InputParametersDataModels != null ? $"{parameters.StoredProcedureInfo.Name}Input" : "EmptyInputParams")}" +
                $"{(parameters.OutputParametersDataModels != null ? $", {parameters.StoredProcedureInfo.Name}Output" : "")}> _dapperExecutor;");
        }

        private static void AppendOutputParameterPropertyField(StoredProcedureParameters parameters,
            StringBuilder outputClass,
            OutputParametersDataModel field)
        {
            outputClass.AppendLine(new string(
                $"\t\t[Newtonsoft.Json.JsonProperty({(field.ParameterName == null ? $"\"{parameters.StoredProcedureInfo.Name}Result\"" : $"\"{field.ParameterName}\"")} " + //If not nullable -> required
                $", Required = {(field.IsNullable ? "Newtonsoft.Json.Required.DisallowNull" : "Newtonsoft.Json.Required.Default")})]\n" + //Json field
                $"\t\tpublic {field.TypeName} " + //Type name
                $"{(field.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{field.ParameterName.Replace("-", "_")}")} " + //Param name
                "{get; set;} \n"));
        }
    }
}