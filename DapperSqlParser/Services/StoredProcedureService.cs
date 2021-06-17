using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using Newtonsoft.Json;

namespace DapperSqlParser.Services
{
    public class StoredProcedureService
    {
        private readonly string _connectionString;

        private readonly Dictionary<string, string> _sqlServerTypesTocSharpTypes = new Dictionary<string, string>
        {
            {"bigint",           typeof(System.Int64).FullName},
            {"binary",           typeof(System.Byte[]).FullName},
            {"bit",              typeof(System.Boolean).FullName},
            {"char",             typeof(System.Char).FullName},
            {"date",             typeof(System.DateTime).FullName},
            {"datetime",         typeof(System.DateTime).FullName},
            {"datetime2",        typeof(System.DateTime).FullName},
            {"datetimeoffset",   typeof(System.DateTimeOffset).FullName},
            {"decimal",          typeof(System.Decimal).FullName},
            {"filestream",       typeof(System.Byte[]).FullName},
            {"float",            typeof(System.Double).FullName},
            {"geography",        "Microsoft.SqlServer.Types.SqlGeography"},
            {"geometry",         "Microsoft.SqlServer.Types.SqlGeometry"},
            {"hierarchyid",      "Microsoft.SqlServer.Types.SqlHierarchyId"},
            {"image",            typeof(System.Byte[]).FullName},
            {"int",              typeof(System.Int32).FullName},
            {"money",            typeof(System.Decimal).FullName},
            {"nchar",            typeof(System.String).FullName},
            {"ntext",            typeof(System.String).FullName},
            {"numeric",          typeof(System.Decimal).FullName},
            {"nvarchar",         typeof(System.String).FullName},
            {"real",             typeof(System.Single).FullName},
            {"rowversion",       typeof(System.Byte[]).FullName},
            {"smalldatetime",    typeof(System.DateTime).FullName},
            {"smallint",         typeof(System.Int16).FullName},
            {"smallmoney",       typeof(System.Decimal).FullName},
            {"sql_variant",      typeof(System.Object).FullName},
            {"text",             typeof(System.String).FullName},
            {"time",             typeof(System.TimeSpan).FullName},
            {"timestamp",        typeof(System.Byte[]).FullName},
            {"tinyint",          typeof(System.Byte).FullName},
            {"uniqueidentifier", typeof(System.Guid).FullName},
            {"varbinary",        typeof(System.Byte[]).FullName},
            {"varchar",          typeof(System.String).FullName},
            {"xml",              typeof(System.String).FullName}
        };

        public StoredProcedureService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<StoredProcedureParameters> GetSpDataAsync(string spName)
        {
            await using var connection = new SqlConnection(_connectionString);

            var tmp = await GetSpTextAsync(spName);

            var values = new {spName};
            const string query =
                "declare @sType varchar(MAX)\n\n\tDECLARE @OutputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT\n\n\t\t);\n\tDECLARE @InputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT\n\n\t\t);\n\n\tINSERT INTO\n\t\t@OutputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length])\n\tSELECT \n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length\n\tFROM sys.dm_exec_describe_first_result_set (@spName, null, 0) ;\n\n\tINSERT INTO\n\t\t@InputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length] )\n\t SELECT\t\n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length\n\tFROM sys.parameters  \n\tWHERE object_id = (OBJECT_ID(@spName)); \n\n\tSELECT\t\n\t\t(SELECT \n\t\t\t\'Definition\'=definition\n\t\t\tFROM sys.sql_modules  \n\t\t\tWHERE object_id = (OBJECT_ID(@spName))\n\t\t\tFOR JSON PATH)[StoredProcedureText],\n\t\t(SELECT   \n\t\t\t\'StoredProcedureName\'=@spName,\n            \'StoredProcedureObjectId\'= OBJECT_ID(@spName)\n\t\t\tFOR JSON PATH)[StoredProcedureInfo],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'=system_type_name,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length\n\t\tFROM @InputParameters iparams FOR JSON PATH)[InputParameters],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'=system_type_name,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length\n\t\t\tFROM @OutputParameters oparams FOR JSON PATH) [OutputParameters]\n\t\tFOR JSON PATH, without_array_wrapper;";
            var queryResultChunks = await connection.QueryAsync<string>(query, values, commandType: CommandType.Text);

            var spParams = JsonConvert.DeserializeObject<StoredProcedureParameters>(string.Join("", queryResultChunks));

            /*
             *  Convert sql types into c# types
             *  If type doesn't exist -> return "object" type 
             */
            if (spParams?.OutputParametersDataModels != null)
                foreach (var outParam in spParams?.OutputParametersDataModels)
                    outParam.TypeName = ConvertSqlServerFormatToCSharp(outParam.TypeName);
            if (spParams?.InputParametersDataModels != null)
                foreach (var inParam in spParams?.InputParametersDataModels)
                    inParam.TypeName = ConvertSqlServerFormatToCSharp(inParam.TypeName);

            if (spParams != null)
                spParams.StoredProcedureInfo.Name =
                    spParams.StoredProcedureInfo.Name
                        .FirstCharToUpper(); //Make first char upper case for consistent c# naming

            return spParams;
        }

        public async Task<StoredProcedureModel[]> GetSpListAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            const string query =
                "SELECT \n\t\t\'StoredProcedureName\'=name,\n\t\t\'StoredProcedureObjectId\'=object_id\n\tFROM [sys].[procedures] sp\n\tWHERE is_ms_shipped = 0\n\tAND NOT EXISTS (\n\t\tselect ep.[major_id]\n\t\tfrom [sys].[extended_properties] ep\n\t\twhere ep.[major_id] = sp.[object_id]\n\t\tand ep.[minor_id] = 0\n\t\tand ep.[class] = 1\n\t\tand ep.[name] = N\'microsoft_database_tools_support\')\n\t\tFOR JSON PATH;";

            var queryResultChunks = await connection.QueryAsync<string>(query,
                commandType: CommandType.Text);

            return JsonConvert.DeserializeObject<StoredProcedureModel[]>(string.Join("", queryResultChunks));
        }

        public async Task<string> GetSpTextAsync(string spName)
        {
            await using var connection = new SqlConnection(_connectionString);

            var values = new {spName};

            const string query =
                "    SELECT definition\n\tFROM sys.sql_modules  \n\tWHERE object_id = (OBJECT_ID(@spName));  ";

            var queryResultChunks = await connection.QuerySingleAsync<string>(query, values,
                commandType: CommandType.Text);

            return string.Join("", queryResultChunks);
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
            return _sqlServerTypesTocSharpTypes.ContainsKey(typeName)
                ? _sqlServerTypesTocSharpTypes[typeName]
                : "System.Object";
        }
    }
}