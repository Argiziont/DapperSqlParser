using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;
using static DapperSqlParser.StoredProcedureCodeGeneration.TemplateService.JsonNamingConstants;

namespace DapperSqlParser.StoredProcedureCodeGeneration.StoredProcedureParsers
{
    public class StoredProcedureInputModelGenerator : ICodeGenerator
    {
        private readonly InputParametersDataModel[] _inputModels;
        private readonly string _storedProcedureDefinition;
        private readonly string _storedProcedureName;

        public StoredProcedureInputModelGenerator(InputParametersDataModel[] inputModels, string storedProcedureName,
            string storedProcedureDefinition)
        {
            _inputModels = inputModels;
            _storedProcedureName = storedProcedureName ?? throw new ArgumentNullException(nameof(storedProcedureName));
            _storedProcedureDefinition = storedProcedureDefinition ??
                                         throw new ArgumentNullException(nameof(storedProcedureDefinition));
        }

        public async Task<string> GenerateAsync()
        {
            return await CreateSpDataModelForInputParams();
        }

        private async Task<string> CreateSpDataModelForInputParams()
        {
            if (_inputModels == null) return await Task.FromResult(string.Empty);

            if (CheckStringForInputJsonSnippets(_storedProcedureDefinition))
                return
                    $"{CodeGeneratorUtils.CreateJsonWrapperClassAttribute(_inputModels.First().ParameterName)} {await CreateSpDataModelForInputParamsJson(_storedProcedureDefinition)}";
            StringBuilder inputClass = new StringBuilder();

            foreach (InputParametersDataModel field in _inputModels)
                inputClass.AppendLine(CodeGeneratorUtils.CreateProperty(
                    field.TypeName, //Append property type
                    field.ParameterName == null
                        ? "Result"
                        : $"{field.ParameterName.Replace("-", "_").Replace("@", "").FirstCharToUpper()}", //Append property name
                    CodeGeneratorUtils.CreateJsonPropertyAttribute(field.ParameterName.Replace("@", ""),
                        field.IsNullable) //Append property attribute
                ));

            return CodeGeneratorUtils.CreateClass(_storedProcedureName + "Input", inputClass.ToString());
        }

        private static async Task<string> CreateSpDataModelForInputParamsJson(string storedProcedureDefinition)
        {
            int jsonSchemaStartIndex =
                storedProcedureDefinition.IndexOf(InputSchemeStartKeyWordSnippet,
                    StringComparison.Ordinal);
            int jsonSchemaEndIndex =
                storedProcedureDefinition.IndexOf(InputSchemeEndKeyWordSnippet,
                    StringComparison.Ordinal);

            if (jsonSchemaStartIndex == -1 && jsonSchemaEndIndex == -1)
                throw new NullModelException();

            jsonSchemaStartIndex += InputSchemeStartKeyWordSnippet.Length;
            jsonSchemaEndIndex -= jsonSchemaStartIndex;

            string jsonSchema = storedProcedureDefinition.Substring(jsonSchemaStartIndex, jsonSchemaEndIndex);

            return await CodeGeneratorUtils.CreateClassFromJsonSchema(jsonSchema);
        }

        private static bool CheckStringForInputJsonSnippets(string storedProcedureDefinition)
        {
            return
                storedProcedureDefinition.IndexOf(InputSchemeStartKeyWordSnippet, StringComparison.Ordinal) != -1 &&
                storedProcedureDefinition.IndexOf(InputSchemeEndKeyWordSnippet, StringComparison.Ordinal) != -1;
        }
    }
}