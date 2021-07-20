using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.StoredProcedureCodeGeneration.TemplateService;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public static class CodeGeneratorUtils
    {
        public static string CreateRegionWithName(string regionName, string regionDefinition)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{Environment.NewLine}{TextLevel.FirstLevel}#region {regionName}");
            stringBuilder.AppendLine($"{regionDefinition}");
            stringBuilder.AppendLine($"{TextLevel.FirstLevel}#endregion");

            return stringBuilder.ToString();
        }
        
        
        public static string CreateStoredProcedureErrorComment(string storedProcedureName, string errorMessage)
        {
            return "//Couldn't parse Stored procedure  with className: " +
                   $"{storedProcedureName} because of internal error: " +
                   $"{errorMessage}\n\t";
        }
        
        public static string CreateStoredProcedureNotFoundComment(string storedProcedureName)
        {
            return $"//Model for {storedProcedureName} was not found, could not parse this Stored Procedure!";
        }

        public static async Task<string> CreateCsSharpClassFromJsonSchema(string jsonSchemaFromSp)
        {
            const string trimStart = "#pragma warning disable // Disable all warnings"; //Not necessary warning disable

            JsonSchema schema = await JsonSchema.FromJsonAsync(jsonSchemaFromSp);
            CSharpGenerator generator = new CSharpGenerator(schema);


            string generatedClasses = generator.GenerateFile();

            int nameSpaceBracketIndex = generatedClasses.IndexOf(trimStart, StringComparison.Ordinal) +
                                        trimStart.Length + 2;
            
            return generatedClasses.Substring(nameSpaceBracketIndex,
                generatedClasses.Length - 2 - nameSpaceBracketIndex);
        }

        public static string CreateClass(string className, string classDefinition)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{TextLevel.FirstLevel}{AccessModifier.Public} {SpecialKeyWord.Class} {className}");
            stringBuilder.AppendLine($"{TextLevel.FirstLevel}{{");
            stringBuilder.AppendLine($"{classDefinition}");
            stringBuilder.AppendLine($"{TextLevel.FirstLevel}}}");

            return stringBuilder.ToString();
        }
        
        public static string CreateClass(string className, IEnumerable<string> classDefinitions)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{TextLevel.FirstLevel}{AccessModifier.Public} {SpecialKeyWord.Class} {className}");
            stringBuilder.AppendLine($"{TextLevel.FirstLevel}{{");
            foreach (string classDefinition in classDefinitions) stringBuilder.AppendLine($"{classDefinition}");
            stringBuilder.AppendLine($"{TextLevel.FirstLevel}}}");

            return stringBuilder.ToString();
        }
        
        public static string CreateProperty(string propertyType, string propertyName, string propertyAttribute=default)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{TextLevel.SecondLevel} {propertyAttribute} {AccessModifier.Public} {propertyType} {propertyName} {SpecialKeyWord.PropertyGetSetAccessor}");

            return stringBuilder.ToString();
        }

        public static string CreateJsonPropertyAttribute(string propertyName, bool isNullable)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"[Newtonsoft.Json.JsonProperty({(propertyName == null ? "\"Result\"" : $"\"{propertyName}\"")} , Required = {(isNullable ? "Newtonsoft.Json.Required.DisallowNull" : "Newtonsoft.Json.Required.Default")})]");

            return stringBuilder.ToString();
        }

        public static string CreateJsonWrapperClassAttribute(string propertyName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{TextLevel.FirstLevel}[JsonWrapper(\"{ propertyName}\")]");

            return stringBuilder.ToString();
        }

        public static string CreateIDapperExecutorField(object inputParameter, object outputParameter, string storedProcedureName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{AccessModifier.Private} {SpecialKeyWord.Readonly} IDapperExecutor<{(inputParameter != null ? $"{storedProcedureName}Input" : "EmptyInputParams")}{(outputParameter != null ? $", {storedProcedureName}Output" : "")}> _dapperExecutor;");

            return stringBuilder.ToString();
        }
        
        public static string CreateDapperClientConstructor(object inputParameter, object outputParameter, string storedProcedureName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{TextLevel.SecondLevel}{AccessModifier.Public} {storedProcedureName}(IDapperExecutor<{(inputParameter != null ? $"{storedProcedureName}Input" : "EmptyInputParams")}{(outputParameter != null ? $", {storedProcedureName}Output" : "")}> dapperExecutor){{");
            stringBuilder.AppendLine($"{TextLevel.ThirdLevel}{SpecialKeyWord.This}._dapperExecutor = dapperExecutor;");
            stringBuilder.AppendLine($"{TextLevel.SecondLevel}}}");


            return stringBuilder.ToString();
        }

        public static string CreateDapperClientMethod(object inputParameter, object outputParameter, string storedProcedureName, bool isReturnTypeJson)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{TextLevel.SecondLevel}public System.Threading.Tasks.Task{(outputParameter != null ? $"<System.Collections.Generic.IEnumerable<{storedProcedureName}Output>>" : " ")}Execute({(inputParameter != null ? $"{storedProcedureName}Input request" : "")} ){{");
            stringBuilder.AppendLine($"{TextLevel.ThirdLevel}return _dapperExecutor.{(isReturnTypeJson ? "ExecuteJsonAsync" : "ExecuteAsync")}(\"{storedProcedureName}\"{(inputParameter != null ? ", request" : "")});");
            stringBuilder.AppendLine($"{TextLevel.SecondLevel}}}");

            return stringBuilder.ToString();
        }
    }
}