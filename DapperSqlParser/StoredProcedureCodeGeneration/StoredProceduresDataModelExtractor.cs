using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using static DapperSqlParser.StoredProcedureCodeGeneration.TemplateService.TemplateNamingConstants;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public  class StoredProceduresDataModelExtractor: IStoredProceduresDataModelExtractor
    {
        public StoredProcedureParameters Parameters { get; set; }

        public StoredProceduresDataModelExtractor(StoredProcedureParameters parameters)
        {
            Parameters = parameters;
        }
        
        public StoredProceduresDataModelExtractor()
        {
        }

        public async Task<string> CreateSpDataModelForOutputParams()
        {
            if (Parameters == null) throw new ArgumentNullException(nameof(Parameters));
            if (Parameters.OutputParametersDataModels == null) return await Task.FromResult(string.Empty);

            StringBuilder outputClass = new StringBuilder();

            if (Parameters.OutputParametersDataModels.First().ParameterName != null && Guid.TryParse(
                Parameters.OutputParametersDataModels.First().ParameterName.Replace("JSON_", ""),
                out _))
                return await CreateSpDataModelForOutputParamsJson();

            //Sp returns fields with data

            outputClass.AppendLine($"\tpublic class {Parameters.StoredProcedureInfo.Name}Output \n\t{{");

            foreach (OutputParametersDataModel field in Parameters.OutputParametersDataModels) //Getter/setter
                outputClass.AppendLine(new string(
                    $"\t\t[Newtonsoft.Json.JsonProperty({(field.ParameterName == null ? $"\"{Parameters.StoredProcedureInfo.Name}Result\"" : $"\"{field.ParameterName}\"")} " +
                    $", Required = {(field.IsNullable ? "Newtonsoft.Json.Required.DisallowNull" : "Newtonsoft.Json.Required.Default")})]\n" + //If fields isn't nullable -> it's required in any case
                    $"\t\tpublic {field.TypeName} " + //Type name
                    $"{(field.ParameterName == null ? $"{Parameters.StoredProcedureInfo.Name}Result" : $"{field.ParameterName.Replace("-", "_")}")} " + //Param name
                    "{get; set;} \n"));
            
            outputClass.Append("\t}\n");

            return await Task.FromResult(outputClass.ToString());
        }

        public async Task<string> CreateSpDataModelForOutputParamsJson()
        {
            int jsonSchemaStartIndex =
                Parameters.StoredProcedureText.Definition.IndexOf(OutputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            int jsonSchemaEndIndex =
                Parameters.StoredProcedureText.Definition.IndexOf(OutputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex == -1 && jsonSchemaEndIndex == -1)
                throw new NullModelException();

            jsonSchemaStartIndex += OutputSchemeStartKeyWordSnippet.Length;
            jsonSchemaEndIndex -= jsonSchemaStartIndex;

            string jsonSchemaFromSp =
                Parameters.StoredProcedureText.Definition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

            //Schema parsing from JSON to csSharp
            JsonSchema schema = await JsonSchema.FromJsonAsync(jsonSchemaFromSp);
            CSharpGenerator generator = new CSharpGenerator(schema);

            const string trimStart = "#pragma warning disable // Disable all warnings";

            string generatedClasses = generator.GenerateFile();

            int nameSpaceBracketIndex = generatedClasses.IndexOf(trimStart, StringComparison.Ordinal) +
                                        trimStart.Length + 2;
            StringBuilder outputClass = new StringBuilder(generatedClasses.Substring(nameSpaceBracketIndex,
                generatedClasses.Length - 2 - nameSpaceBracketIndex));


            return await Task.FromResult(outputClass.ToString());
        }

        public async Task<string> CreateSpDataModelForInputParams()
        {
            if (Parameters == null) throw new ArgumentNullException(nameof(Parameters));
            if (Parameters.InputParametersDataModels == null) return await Task.FromResult(string.Empty);

            StringBuilder inputClass = new StringBuilder();


            int jsonSchemaStartIndex =
                Parameters.StoredProcedureText.Definition.IndexOf(InputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            int jsonSchemaEndIndex =
                Parameters.StoredProcedureText.Definition.IndexOf(InputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex != -1 && jsonSchemaEndIndex != -1)
                return await CreateSpDataModelForInputParamsJson();

            inputClass.AppendLine($"\tpublic class {Parameters.StoredProcedureInfo.Name}Input \n\t{{");

            foreach (InputParametersDataModel field in Parameters.InputParametersDataModels) //Getter/setter
                inputClass.AppendLine(new string(
                    $"\t\t{(field.ParameterName == null ? "" : $"[Newtonsoft.Json.JsonProperty(\"{field.ParameterName.Replace("@", "")}\")]")} " + //If not nullable -> required
                    $"{(field.IsNullable ? "" : "[System.ComponentModel.DataAnnotations.Required()] ")}" + //Json field
                    $"public {field.TypeName} " + //Type name
                    $"{(field.ParameterName == null ? $"{Parameters.StoredProcedureInfo.Name}Result" : $"{field.ParameterName.Replace("-", "_").Replace("@", "").FirstCharToUpper()}")} " + //Param name
                    "{get; set;} \n"));


            inputClass.Append("\t}\n");
            return await Task.FromResult(inputClass.ToString());
        }

        public async Task<string> CreateSpDataModelForInputParamsJson()
        {
            int jsonSchemaStartIndex =
                Parameters.StoredProcedureText.Definition.IndexOf(InputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            int jsonSchemaEndIndex =
                Parameters.StoredProcedureText.Definition.IndexOf(InputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex == -1 && jsonSchemaEndIndex == -1)
                throw new NullModelException();

            jsonSchemaStartIndex += InputSchemeStartKeyWordSnippet.Length;
            jsonSchemaEndIndex -= jsonSchemaStartIndex;

            string jsonSchemaFromSp =
                Parameters.StoredProcedureText.Definition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

            //Schema parsing from JSON to csSharp
            JsonSchema schema = await JsonSchema.FromJsonAsync(jsonSchemaFromSp);
            CSharpGenerator generator = new CSharpGenerator(schema);

            const string trimStart = "#pragma warning disable // Disable all warnings";

            string generatedClasses = generator.GenerateFile();

            int nameSpaceBracketIndex =
                generatedClasses.IndexOf(trimStart, StringComparison.Ordinal) + trimStart.Length + 2;
            StringBuilder inputClass = new StringBuilder(
                $"\t[JsonWrapper(\"{Parameters.InputParametersDataModels.First().ParameterName}\")]\n" +
                generatedClasses.Substring(nameSpaceBracketIndex,
                    generatedClasses.Length - 2 - nameSpaceBracketIndex));

            return await Task.FromResult(inputClass.ToString());
        }
    }
}