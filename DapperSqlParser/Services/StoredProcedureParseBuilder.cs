using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;

namespace DapperSqlParser.Services
{
    public static class StoredProcedureParseBuilder
    {
        public static async Task AppendExtractedCsSharpCode(StoredProcedureParameters spParameter,
            StringBuilder outputGeneratedCode)
        {
            string outputModelClass =
                await StoredProceduresDataModelExtractor.CreateSpDataModelForOutputParams(spParameter);
            string inputModelClass =
                await StoredProceduresDataModelExtractor.CreateSpDataModelForInputParams(spParameter);

            string clientClass = await AppendSpClientClass(spParameter);

            outputGeneratedCode.AppendLine(outputModelClass);
            outputGeneratedCode.AppendLine(inputModelClass);
            outputGeneratedCode.AppendLine(clientClass);
        }

        public static void AppendStoredProcedureRegionStart(string regionName,
            StringBuilder outputGeneratedCode)
        {
            outputGeneratedCode.AppendLine(
                $"\n\t#region {regionName}"); //Wrapping every sp into region
        }

        public static void AppendStoredProcedureRegionEnd(StringBuilder outputGeneratedCode)
        {
            outputGeneratedCode.AppendLine("\t#endregion");
        }

        public static void AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo,
            StringBuilder outputGeneratedCode)
        {
            outputGeneratedCode.AppendLine("//Couldn't parse Stored procedure  with name: " +
                                           $"{storedProcedureInfo.Name} because of internal error: " +
                                           $"{storedProcedureInfo.Error}\n\t#endregion");
        }

        public static void AppendStoredProcedureNotFoundMessage(string storedProcedureName,
            StringBuilder outputGeneratedCode)
        {
            outputGeneratedCode.AppendLine(
                $"//Model for {storedProcedureName} was not found, could not parse this Stored Procedure!");
        }

        public static void AppendInputParameterPropertyField(StoredProcedureParameters parameters,
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

        public static void AppendOutputParameterPropertyField(StoredProcedureParameters parameters,
            StringBuilder outputClass,
            OutputParametersDataModel field)
        {
            outputClass.AppendLine(new string(
                $"\t\t[Newtonsoft.Json.JsonProperty({(field.ParameterName == null ? $"\"{parameters.StoredProcedureInfo.Name}Result\"" : $"\"{field.ParameterName}\"")} " +
                $", Required = {(field.IsNullable ? "Newtonsoft.Json.Required.DisallowNull" : "Newtonsoft.Json.Required.Default")})]\n" + //If fields isn't nullable -> it's required in any case
                $"\t\tpublic {field.TypeName} " + //Type name
                $"{(field.ParameterName == null ? $"{parameters.StoredProcedureInfo.Name}Result" : $"{field.ParameterName.Replace("-", "_")}")} " + //Param name
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

        private static bool StoreProcedureInputIsJson(string inputParameterName)
        {
            return inputParameterName != null && Guid.TryParse(inputParameterName.Replace("JSON_", ""), out _);
        }

        private static async Task<string> AppendSpClientClass(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            StringBuilder outputClass = new StringBuilder();

            bool spReturnJsonFlag =
                StoreProcedureInputIsJson(parameters.OutputParametersDataModels?.First().ParameterName);

            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name} \n\t{{"); //Class name

            AppendIDapperExecutorField(parameters, outputClass);
            AppendClientConstructor(parameters, outputClass);
            AppendExecutorMethod(parameters, outputClass, spReturnJsonFlag);

            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
    }
}