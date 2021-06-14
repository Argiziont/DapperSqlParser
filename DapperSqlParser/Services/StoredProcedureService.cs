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
        private readonly string[] _sqlServerTypes =
        {
            "bigint", "binary", "bit", "char", "date", "datetime", "datetime2", "datetimeoffset", "decimal",
            "filestream", "float", "geography", "geometry", "hierarchyid", "image", "int", "money", "nchar", "ntext",
            "numeric", "nvarchar", "real", "rowversion", "smalldatetime", "smallint", "smallmoney", "sql_variant",
            "text", "time", "timestamp", "tinyint", "uniqueidentifier", "varbinary", "varchar", "xml"
        };
        private readonly string[] _cSharpTypes =
        {
            "System.Int64", "System.Byte[]", "System.Boolean", "System.Char", "System.DateTime", "System.DateTime",
            "System.DateTime", "System.DateTimeOffset", "System.Decimal", "System.Byte[]", "System.Double",
            "Microsoft.SqlServer.Types.SqlGeography", "Microsoft.SqlServer.Types.SqlGeometry",
            "Microsoft.SqlServer.Types.SqlHierarchyId", "System.Byte[]", "System.Int32", "System.Decimal", "System.String",
            "System.String", "System.Decimal", "System.String", "System.Single", "System.Byte[]", "System.DateTime",
            "System.Int16", "System.Decimal", "System.Object", "System.String",
            "System.TimeSpan", "System.Byte[]", "System.Byte", "System.Guid", "System.Byte[]", "System.String",
            "System.String"
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
            var index = Array.IndexOf(_sqlServerTypes, typeName);

            return index > -1
                ? _cSharpTypes[index]
                : "System.Object";
        }

    }
}


