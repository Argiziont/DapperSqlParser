using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;
using static DapperSqlParser.StoredProcedureCodeGeneration.TemplateService.JsonNamingConstants;

namespace DapperSqlParser.StoredProcedureCodeGeneration.StoredProcedureParsers
{
    public class StoredProcedureOutputModelGenerator : ICodeGenerator
    {
        private readonly OutputParametersDataModel[] _outputModels;
        private readonly string _storedProcedureDefinition;
        private readonly string _storedProcedureName;

        public StoredProcedureOutputModelGenerator(OutputParametersDataModel[] outputModels, string storedProcedureName,
            string storedProcedureDefinition)
        {
            _outputModels = outputModels;
            _storedProcedureName = storedProcedureName ?? throw new ArgumentNullException(nameof(storedProcedureName));
            _storedProcedureDefinition = storedProcedureDefinition ??
                                         throw new ArgumentNullException(nameof(storedProcedureDefinition));
        }

        public async Task<string> GenerateAsync()
        {
            return await CreateSpDataModelForOutputParams();
        }

        private async Task<string> CreateSpDataModelForOutputParams()
        {
            if (_outputModels == null) return await Task.FromResult(string.Empty);

            if (CheckStringForOutputJsonSnippets(_outputModels.First().ParameterName))
                return await CreateSpDataModelForOutputParamsJson(_storedProcedureDefinition);

            StringBuilder outputProperties = new StringBuilder();

            foreach (OutputParametersDataModel field in _outputModels)
                outputProperties.AppendLine(CodeGeneratorUtils.CreateProperty(
                    field.TypeName, //Append property type
                    field.ParameterName == null
                        ? $"{_storedProcedureName}Result"
                        : $"{field.ParameterName.Replace("-", "_")}", //Append property name
                    CodeGeneratorUtils.CreateJsonPropertyAttribute(field.ParameterName,
                        field.IsNullable) //Append property attribute
                ));

            return CodeGeneratorUtils.CreateClass(_storedProcedureName + "Output", outputProperties.ToString());
        }

        private static async Task<string> CreateSpDataModelForOutputParamsJson(string storedProcedureDefinition)
        {
            int jsonSchemaStartIndex =
                storedProcedureDefinition.IndexOf(OutputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            int jsonSchemaEndIndex =
                storedProcedureDefinition.IndexOf(OutputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex == -1 && jsonSchemaEndIndex == -1)
                throw new NullModelException();

            jsonSchemaStartIndex += OutputSchemeStartKeyWordSnippet.Length;
            jsonSchemaEndIndex -= jsonSchemaStartIndex;

            string jsonSchema = storedProcedureDefinition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

            return await CodeGeneratorUtils.CreateCsSharpClassFromJsonSchema(jsonSchema);
        }

        private static bool CheckStringForOutputJsonSnippets(string jsonParameterName)
        {
            return jsonParameterName != null && Guid.TryParse(
                jsonParameterName.Replace("JSON_", ""),
                out _);
        }
    }
}