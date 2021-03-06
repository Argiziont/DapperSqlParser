using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.TemplateService;
using Newtonsoft.Json;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public class StoredProcedureService
    {
        private readonly string _connectionString;

        public StoredProcedureService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<StoredProcedureParameters> GetSpDataAsync(string spName)
        {
            await using SqlConnection connection = new SqlConnection(_connectionString);

            var values = new {spName};
            const string query =
                "-- SET NOCOUNT ON added to prevent extra result sets from\n\t-- interfering with SELECT statements.\n\tSET NOCOUNT ON;\n\n\tdeclare @sType varchar(MAX)\n\n\tDECLARE @OutputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT,\n\t\t\t[error_number] INT,\n\t\t\t[error_message] NVARCHAR(MAX)\n\t\t);\n\tDECLARE @InputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT\n\n\t\t);\n\n\tINSERT INTO\n\t\t@OutputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length],\n\t\t[error_number],\n\t\t[error_message])\n\tSELECT \n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length,\n\t\terror_type,\n\t\terror_message\t\t\n\tFROM sys.dm_exec_describe_first_result_set (@spName, null, 0) ;\n\n\tINSERT INTO\n\t\t@InputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length] )\n\t SELECT\t\n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length\n\tFROM sys.parameters  \n\tWHERE object_id = (OBJECT_ID(@spName)); \n\n\tSELECT\t\n\t\t(SELECT \n\t\t\t\'Definition\'=definition\n\t\t\tFROM sys.sql_modules  \n\t\t\tWHERE object_id = (OBJECT_ID(@spName))\n\t\t\tFOR JSON PATH)[StoredProcedureText],\n\t\t(SELECT   \n\t\t\t\'StoredProcedureName\'=@spName,\n            \'StoredProcedureObjectId\'= OBJECT_ID(@spName)\n\t\t\tFOR JSON PATH)[StoredProcedureInfo],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'=system_type_name,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length\n\t\tFROM @InputParameters iparams FOR JSON PATH)[InputParameters],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'=system_type_name,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length,\n\t\t\t\'ErrorMessage\'= error_message,\n\t\t\t\'ErrorNumber\'= error_number\n\t\t\tFROM @OutputParameters oparams FOR JSON PATH) [OutputParameters]\n\t\tFOR JSON PATH, without_array_wrapper;";
            var queryResultChunks = await connection.QueryAsync<string>(query, values, commandType: CommandType.Text);

            StoredProcedureParameters storedProcedureData =
                JsonConvert.DeserializeObject<StoredProcedureParameters>(string.Join("", queryResultChunks));

            /*
             *  Convert sql types into c# types
             *  If type doesn't exist -> return "object" type
             *  If this fails means that we can't parse this sp
             */
            if (storedProcedureData?.OutputParametersDataModels?.FirstOrDefault() != null &&
                storedProcedureData.OutputParametersDataModels.First().ErrorCode == 10)
            {
                int jsonSchemaStartIndex =
                    storedProcedureData.StoredProcedureText.Definition.IndexOf(
                        JsonNamingConstants.OutputSchemeStartKeyWordSnippet, StringComparison.Ordinal);
                int jsonSchemaEndIndex =
                    storedProcedureData.StoredProcedureText.Definition.IndexOf(
                        JsonNamingConstants.OutputSchemeEndKeyWordSnippet, StringComparison.Ordinal);

                if (jsonSchemaStartIndex != -1 && jsonSchemaEndIndex != -1)
                {
                    storedProcedureData.OutputParametersDataModels.First().ParameterName = "JSON_" + Guid.NewGuid();
                    storedProcedureData.OutputParametersDataModels.First().TypeName = "nvarchar";
                    storedProcedureData.OutputParametersDataModels.First().MaxLength = -1;
                }
                else
                {
                    storedProcedureData.StoredProcedureInfo.Error =
                        storedProcedureData.OutputParametersDataModels.First().ErrorMessage;
                    return storedProcedureData;
                }
            }

            if (storedProcedureData?.OutputParametersDataModels != null)
                foreach (OutputParametersDataModel outParam in storedProcedureData.OutputParametersDataModels)
                    outParam.TypeName = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(outParam.TypeName);
            if (storedProcedureData?.InputParametersDataModels != null)
                foreach (InputParametersDataModel inParam in storedProcedureData.InputParametersDataModels)
                    inParam.TypeName = SqlCsSharpTypesConverter.ConvertSqlServerFormatToCSharp(inParam.TypeName);

            if (storedProcedureData != null)
                storedProcedureData.StoredProcedureInfo.Name =
                    storedProcedureData.StoredProcedureInfo.Name
                        .FirstCharToUpper(); //Make first char upper case for consistent c# naming


            return storedProcedureData;
        }

        public async Task<StoredProcedureModel[]> GetSpListAsync()
        {
            await using SqlConnection connection = new SqlConnection(_connectionString);

            const string query =
                "SELECT \n\t\t\'StoredProcedureName\'=name,\n\t\t\'StoredProcedureObjectId\'=object_id\n\tFROM [sys].[procedures] sp\n\tWHERE is_ms_shipped = 0\n\tAND NOT EXISTS (\n\t\tselect ep.[major_id]\n\t\tfrom [sys].[extended_properties] ep\n\t\twhere ep.[major_id] = sp.[object_id]\n\t\tand ep.[minor_id] = 0\n\t\tand ep.[class] = 1\n\t\tand ep.[name] = N\'microsoft_database_tools_support\')\n\t\tFOR JSON PATH;";


            var queryResultChunks = await connection.QueryAsync<string>(query,
                commandType: CommandType.Text);

            return JsonConvert.DeserializeObject<StoredProcedureModel[]>(string.Join("", queryResultChunks));
        }

        public async Task<List<StoredProcedureParameters>> GenerateModelsListAsync()
        {
            var spList = await GetSpListAsync();
            var paramsList = new List<StoredProcedureParameters>();

            foreach (StoredProcedureModel sp in spList)
                try
                {
                    StoredProcedureParameters spParameter = await GetSpDataAsync(sp.Name);
                    paramsList.Add(spParameter);
                }
                catch
                {
                    /* ignored*/
                }

            return paramsList;
        }

        public async Task<List<StoredProcedureParameters>> GenerateModelsListAsync(params string[] storedProcedureNames)
        {
            var paramsList = new List<StoredProcedureParameters>();

            foreach (string storedProcedureName in storedProcedureNames)
                try
                {
                    StoredProcedureParameters spParameter = await GetSpDataAsync(storedProcedureName);
                    paramsList.Add(spParameter);
                }
                catch
                {
                    /* ignored*/
                }

            return paramsList;
        }
    }
}