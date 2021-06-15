using Dapper;
using DapperSqlParser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperSqlParser.Services
{
    public class StoredProcedureService
    {
        private readonly string _connectionString;

        private readonly Dictionary<string, string> _sqlServerTypesTocSharpTypes = new Dictionary<string, string>()
        {
            {"bigint", "System.Int64"},
            {"binary", "System.Byte[]"},
            {"bit", "System.Boolean"},
            {"char", "System.Char"},
            {"date", "System.DateTime"},
            {"datetime", "System.DateTime"},
            {"datetime2", "System.DateTime"},
            {"datetimeoffset", "System.DateTimeOffset"},
            {"decimal", "System.Decimal"},
            {"filestream", "System.Byte[]"},
            {"float", "System.Double"},
            {"geography", "Microsoft.SqlServer.Types.SqlGeography"},
            {"geometry", "Microsoft.SqlServer.Types.SqlGeometry"},
            {"hierarchyid", "Microsoft.SqlServer.Types.SqlHierarchyId"},
            {"image", "System.Byte[]"},
            {"int", "System.Int32"},
            {"money", "System.Decimal"},
            {"nchar", "System.String"},
            {"ntext", "System.String"},
            {"numeric", "System.Decimal"},
            {"nvarchar", "System.String"},
            {"real", "System.Single"},
            {"rowversion", "System.Byte[]"},
            {"smalldatetime", "System.DateTime"},
            {"smallint", "System.Int16"},
            {"smallmoney", "System.Decimal"},
            {"sql_variant", "System.Object"},
            {"text", "System.String"},
            {"time", "System.TimeSpan"},
            {"timestamp", "System.Byte[]"},
            {"tinyint", "System.Byte"},
            {"uniqueidentifier", "System.Guid"},
            {"varbinary", "System.Byte[]"},
            {"varchar", "System.String"},
            {"xml", "System.String"}
        };

        public StoredProcedureService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<StoredProcedureParameters> GetSpDataAsync(string spName)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var values = new { spName };
            const string query = "declare @sType varchar(MAX)\n\n\tDECLARE @OutputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT\n\n\t\t);\n\tDECLARE @InputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT\n\n\t\t);\n\n\tINSERT INTO\n\t\t@OutputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length])\n\tSELECT \n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length\n\tFROM sys.dm_exec_describe_first_result_set (@spName, null, 0) ;\n\n\tINSERT INTO\n\t\t@InputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length] )\n\t SELECT\t\n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length\n\tFROM sys.parameters  \n\tWHERE object_id = (OBJECT_ID(@spName)); \n\n\tSELECT\t\n\t\t(SELECT   \n\t\t\t\'StoredProcedureName\'=@spName,\n            \'StoredProcedureObjectId\'= OBJECT_ID(@spName)\n\t\t\tFOR JSON PATH)[StoredProcedureInfo],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'= system_type_name,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length\n\t\tFROM @InputParameters iparams FOR JSON PATH)[InputParameters],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'= system_type_name,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length\n\t\t\tFROM @OutputParameters oparams FOR JSON PATH) [OutputParameters]\n\t\tFOR JSON PATH, without_array_wrapper;";
            
            var queryResultChunks = await connection.QueryAsync<string>(query, values, commandType: CommandType.Text);

            var spParams = JsonConvert.DeserializeObject<StoredProcedureParameters>(string.Join("", queryResultChunks));

            if (spParams?.OutputParametersDataModels!=null)
                foreach (var outParam in spParams?.OutputParametersDataModels)
                {
                    outParam.TypeName = ConvertSqlServerFormatToCSharp(outParam.TypeName);
                }
            if (spParams?.InputParametersDataModels != null)
                foreach (var inParam in spParams?.InputParametersDataModels)
                {
                    inParam.TypeName = ConvertSqlServerFormatToCSharp(inParam.TypeName);
                }

            return spParams;

        }
        public async Task<StoredProcedureModel[]> GetSpListAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            const string query = "SELECT \n\t\t\'StoredProcedureName\'=name,\n\t\t\'StoredProcedureObjectId\'=object_id\n\tFROM [sys].[procedures] sp\n\tWHERE is_ms_shipped = 0\n\tAND NOT EXISTS (\n\t\tselect ep.[major_id]\n\t\tfrom [sys].[extended_properties] ep\n\t\twhere ep.[major_id] = sp.[object_id]\n\t\tand ep.[minor_id] = 0\n\t\tand ep.[class] = 1\n\t\tand ep.[name] = N\'microsoft_database_tools_support\')\n\t\tFOR JSON PATH;";

            var queryResultChunks = await connection.QueryAsync<string>(query,
            commandType: CommandType.Text);

            return JsonConvert.DeserializeObject<StoredProcedureModel[]>(string.Join("", queryResultChunks));
        }

        public async Task<List<StoredProcedureParameters>> GenerateModelsListAsync()
        {
            var spList = await GetSpListAsync();
            var paramsList = new List<StoredProcedureParameters>();

            foreach (var sp in spList)
            {
                var spParameter = await GetSpDataAsync(sp.Name);
                paramsList.Add(spParameter);
            }

            return paramsList;
        }

        public string ConvertSqlServerFormatToCSharp(string typeName)
        {
            return _sqlServerTypesTocSharpTypes.ContainsKey(typeName) ? _sqlServerTypesTocSharpTypes[typeName] : "System.Object";
        }

    }
}