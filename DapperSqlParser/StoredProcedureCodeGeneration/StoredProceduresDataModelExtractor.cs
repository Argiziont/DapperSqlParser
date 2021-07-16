using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Models;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using static DapperSqlParser.StoredProcedureCodeGeneration.StoredProcedureParseBuilder;
using static DapperSqlParser.StoredProcedureCodeGeneration.TemplateService.TemplateNamingConstants;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public static class StoredProceduresDataModelExtractor
    {
        public static async Task<string> CreateSpDataModelForOutputParams(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parameters.OutputParametersDataModels == null) return await Task.FromResult(string.Empty);

            StringBuilder outputClass = new StringBuilder();

            if (parameters.OutputParametersDataModels.First().ParameterName != null && Guid.TryParse(
                parameters.OutputParametersDataModels.First().ParameterName.Replace("JSON_", ""),
                out _))
                return await CreateSpDataModelForOutputParamsJson(parameters);

            //Sp returns fields with data

            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Output \n\t{{");

            foreach (OutputParametersDataModel field in parameters.OutputParametersDataModels) //Getter/setter
                AppendOutputParameterPropertyField(parameters, outputClass, field);

            outputClass.Append("\t}\n");

            return await Task.FromResult(outputClass.ToString());
        }

        private static async Task<string> CreateSpDataModelForOutputParamsJson(StoredProcedureParameters parameters)
        {
            int jsonSchemaStartIndex =
                parameters.StoredProcedureText.Definition.IndexOf(OutputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            int jsonSchemaEndIndex =
                parameters.StoredProcedureText.Definition.IndexOf(OutputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex == -1 && jsonSchemaEndIndex == -1)
                throw new NullModelException();

            jsonSchemaStartIndex += OutputSchemeStartKeyWordSnippet.Length;
            jsonSchemaEndIndex -= jsonSchemaStartIndex;

            string jsonSchemaFromSp =
                parameters.StoredProcedureText.Definition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

            //Schema parsing from JSON to csSharp
            JsonSchema schema = await JsonSchema.FromJsonAsync(jsonSchemaFromSp);
            CSharpGenerator generator = new CSharpGenerator(schema);

            string trimStart = "#pragma warning disable // Disable all warnings";

            string generatedClasses = generator.GenerateFile();

            int nameSpaceBracketIndex = generatedClasses.IndexOf(trimStart, StringComparison.Ordinal) +
                                        trimStart.Length + 2;
            StringBuilder outputClass = new StringBuilder(generatedClasses.Substring(nameSpaceBracketIndex,
                generatedClasses.Length - 2 - nameSpaceBracketIndex));


            return await Task.FromResult(outputClass.ToString());
        }

        public static async Task<string> CreateSpDataModelForInputParams(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parameters.InputParametersDataModels == null) return await Task.FromResult(string.Empty);

            StringBuilder inputClass = new StringBuilder();


            int jsonSchemaStartIndex =
                parameters.StoredProcedureText.Definition.IndexOf(InputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            int jsonSchemaEndIndex =
                parameters.StoredProcedureText.Definition.IndexOf(InputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex != -1 && jsonSchemaEndIndex != -1)
                return await CreateSpDataModelForInputParamsJson(parameters, jsonSchemaStartIndex, jsonSchemaEndIndex);

            inputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name}Input \n\t{{");

            foreach (InputParametersDataModel field in parameters.InputParametersDataModels) //Getter/setter
                AppendInputParameterPropertyField(parameters, inputClass, field);


            inputClass.Append("\t}\n");
            return await Task.FromResult(inputClass.ToString());
        }

        private static async Task<string> CreateSpDataModelForInputParamsJson(StoredProcedureParameters parameters,
            int jsonSchemaStartIndex, int jsonSchemaEndIndex)
        {
            jsonSchemaStartIndex += InputSchemeStartKeyWordSnippet.Length;
            jsonSchemaEndIndex -= jsonSchemaStartIndex;

            string jsonSchemaFromSp =
                parameters.StoredProcedureText.Definition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

            //Schema parsing from JSON to csSharp
            JsonSchema schema = await JsonSchema.FromJsonAsync(jsonSchemaFromSp);
            CSharpGenerator generator = new CSharpGenerator(schema);

            const string trimStart = "#pragma warning disable // Disable all warnings";

            string generatedClasses = generator.GenerateFile();

            int nameSpaceBracketIndex =
                generatedClasses.IndexOf(trimStart, StringComparison.Ordinal) + trimStart.Length + 2;
            StringBuilder inputClass = new StringBuilder(
                $"\t[JsonWrapper(\"{parameters.InputParametersDataModels.First().ParameterName}\")]\n" +
                generatedClasses.Substring(nameSpaceBracketIndex,
                    generatedClasses.Length - 2 - nameSpaceBracketIndex));

            return await Task.FromResult(inputClass.ToString());
        }
    }
}