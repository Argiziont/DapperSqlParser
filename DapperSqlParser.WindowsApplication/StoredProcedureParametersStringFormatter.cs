using DapperSqlParser.Models;
using DapperSqlParser.Services;
using System.Linq;
using DapperSqlParser.StoredProcedureCodeGeneration;

namespace DapperSqlParser.WindowsApplication
{
    public static class StoredProcedureParametersStringFormatter
    {
        public static string FormatInputStoredProcedureParameters(InputParametersDataModel[] inputParameters)
        {
            return inputParameters == null
                ? "Input parameters are empty"
                : inputParameters.Aggregate("",
                    (current, inputParameter) =>
                        current + inputParameter.ParameterName + " " + SqlCsSharpTypesConverter.ConvertCSharpToSqlServerFormat(inputParameter.TypeName) + " \n");
        }

        public static string FormatOutputStoredProcedureParameters(OutputParametersDataModel[] outputParameters)
        {
            return outputParameters == null
                ? "Output parameters are empty"
                : outputParameters.Aggregate("",
                    (current, outputParameter) =>
                        current + outputParameter.ParameterName + " " + SqlCsSharpTypesConverter.ConvertCSharpToSqlServerFormat(outputParameter.TypeName) + " \n");
        }

        public static string FormatStoreProcedureInfo(StoredProcedureInfo storedProcedureParameters)
        {
            return storedProcedureParameters == null
                ? "No details"
                : storedProcedureParameters.Error ?? $"Store procedure has Id: {storedProcedureParameters.Id} and Name: {storedProcedureParameters.Name}";


        }
    }
}