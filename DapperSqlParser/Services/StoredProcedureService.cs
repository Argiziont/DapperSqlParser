using Dapper;
using DapperSqlParser.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperSqlParser.Services
{
    public class StoredProcedureService
    {
        private readonly string _connectionString;

        public StoredProcedureService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<StoredProcedureParameters> GetSpDataAsync(string spName)
        {
            await using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var values = new { spName };
            const string query = "declare @sType varchar(MAX)\n\n\tDECLARE @OutputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT\n\n\t\t);\n\tDECLARE @InputParameters TABLE\n\t\t(\n\t\t\t[id] INT IDENTITY(1,1),\n            [sp_name] NVARCHAR(MAX),\n            [sp_id] INT,\n\t\t\t[system_type_name] NVARCHAR(100),\n\t\t\t[parameter_name]  NVARCHAR(100),\n\t\t\t[is_nullable] BIT,\n\t\t\t[max_length] INT\n\n\t\t);\n\n\tINSERT INTO\n\t\t@OutputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length])\n\tSELECT \n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length\n\tFROM sys.dm_exec_describe_first_result_set (@spName, null, 0) ;\n\n\tINSERT INTO\n\t\t@InputParameters (\n        [sp_name],\n        [sp_id],\n\t\t[system_type_name],\n\t\t[parameter_name],\n\t\t[is_nullable],\n\t\t[max_length] )\n\t SELECT\t\n        @spName,\n        OBJECT_ID(@spName),\n\t\ttype_name(system_type_id),\n\t\tname,\n\t\tis_nullable,\n\t\tmax_length\n\tFROM sys.parameters  \n\tWHERE object_id = (OBJECT_ID(@spName)); \n\n\tSELECT\t\n\t\t(SELECT   \n\t\t\t\'StoredProcedureName\'=@spName,\n            \'StoredProcedureObjectId\'= OBJECT_ID(@spName)\n\t\t\tFOR JSON PATH)[StoredProcedureInfo],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'=\n\t\t\tcase system_type_name\n\t\t\t\twhen \'bigint\'  then \'System.Int64\'\n\t\t\t\twhen \'binary\' then \'System.Byte[]\'\n\t\t\t\twhen \'bit\' then \'System.Boolean\'\n\t\t\t\twhen \'char\' then \'System.String\'\n\t\t\t\twhen \'date\' then \'System.DateTime\'\n\t\t\t\twhen \'datetime\' then \'System.DateTime\'\n\t\t\t\twhen \'datetime2\' then \'System.DateTime\'\n\t\t\t\twhen \'datetimeoffset\' then \'System.DateTimeOffset\'\n\t\t\t\twhen \'decimal\' then \'System.Decimal\'\n\t\t\t\twhen \'filestream\' then \'System.Byte[]\'\n\t\t\t\twhen \'float\' then \'System.Double\'\n\t\t\t\twhen \'image\' then \'System.Byte[]\'\n\t\t\t\twhen \'int\'  then \'System.Int32\'\n\t\t\t\twhen \'money\' then \'System.Decimal\'\n\t\t\t\twhen \'nchar\' then \'System.String\'\n\t\t\t\twhen \'varchar\' then \'System.String\'\n\t\t\t\twhen \'nvarchar\' then \'System.String\'\n\t\t\t\twhen \'ntext\' then \'System.String\'\n\t\t\t\twhen \'numeric\' then \'System.Decimal\'\n\t\t\t\twhen \'real\' then \'System.Single\'\n\t\t\t\twhen \'rowversion\' then \'System.Byte[]\'\n\t\t\t\twhen \'smalldatetime\' then \'System.DateTime\'\n\t\t\t\twhen \'smallint\' then \'System.Int16\'\n\t\t\t\twhen \'smallmoney\' then \'System.Decimal\'\n\t\t\t\twhen \'sql_variant\' then \'System.Object\'\n\t\t\t\twhen \'text\' then \'System.String\'\n\t\t\t\twhen \'time\' then \'System.TimeSpan\'\n\t\t\t\twhen \'timestamp\' then \'System.Byte[]\'\n\t\t\t\twhen \'tinyint\' then \'System.Byte\'\n\t\t\t\twhen \'uniqueidentifier\' then \'System.Guid\'\n\t\t\t\twhen \'varbinary\' then \'System.Byte[]\'\n\t\t\t\twhen \'xml\' then \'System.String\'\n\t\t\t\telse \'System.Object\'\n\t\t\t\tEND,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length\n\t\tFROM @InputParameters iparams FOR JSON PATH)[InputParameters],\n\t\t(SELECT \n            \'StoredProcedureName\'=sp_name,\n            \'StoredProcedureObjectId\'=sp_id,\n\t\t\t\'SystemTypeName\'=\n\t\t\tcase system_type_name\n\t\t\t\twhen \'bigint\'  then \'System.Int64\'\n\t\t\t\twhen \'binary\' then \'System.Byte[]\'\n\t\t\t\twhen \'bit\' then \'System.Boolean\'\n\t\t\t\twhen \'char\' then \'System.String\'\n\t\t\t\twhen \'date\' then \'System.DateTime\'\n\t\t\t\twhen \'datetime\' then \'System.DateTime\'\n\t\t\t\twhen \'datetime2\' then \'System.DateTime\'\n\t\t\t\twhen \'datetimeoffset\' then \'System.DateTimeOffset\'\n\t\t\t\twhen \'decimal\' then \'System.Decimal\'\n\t\t\t\twhen \'filestream\' then \'System.Byte[]\'\n\t\t\t\twhen \'float\' then \'System.Double\'\n\t\t\t\twhen \'image\' then \'System.Byte[]\'\n\t\t\t\twhen \'int\'  then \'System.Int32\'\n\t\t\t\twhen \'money\' then \'System.Decimal\'\n\t\t\t\twhen \'nchar\' then \'System.String\'\n\t\t\t\twhen \'varchar\' then \'System.String\'\n\t\t\t\twhen \'nvarchar\' then \'System.String\'\n\t\t\t\twhen \'ntext\' then \'System.String\'\n\t\t\t\twhen \'numeric\' then \'System.Decimal\'\n\t\t\t\twhen \'real\' then \'System.Single\'\n\t\t\t\twhen \'rowversion\' then \'System.Byte[]\'\n\t\t\t\twhen \'smalldatetime\' then \'System.DateTime\'\n\t\t\t\twhen \'smallint\' then \'System.Int16\'\n\t\t\t\twhen \'smallmoney\' then \'System.Decimal\'\n\t\t\t\twhen \'sql_variant\' then \'System.Object\'\n\t\t\t\twhen \'text\' then \'System.String\'\n\t\t\t\twhen \'time\' then \'System.TimeSpan\'\n\t\t\t\twhen \'timestamp\' then \'System.Byte[]\'\n\t\t\t\twhen \'tinyint\' then \'System.Byte\'\n\t\t\t\twhen \'uniqueidentifier\' then \'System.Guid\'\n\t\t\t\twhen \'varbinary\' then \'System.Byte[]\'\n\t\t\t\twhen \'xml\' then \'System.String\'\n\t\t\t\telse \'System.Object\'\n\t\t\t\tEND,\n\t\t\t\'ParameterName\'=parameter_name,\n\t\t\t\'IsNullable\'=is_nullable,\n\t\t\t\'MaxLength\'= max_length\n\t\t\tFROM @OutputParameters oparams FOR JSON PATH) [OutputParameters]\n\t\tFOR JSON PATH, without_array_wrapper;";
            var queryResultChunks = await connection.QueryAsync<string>(query, values,
            commandType: CommandType.Text);

            return JsonConvert.DeserializeObject<StoredProcedureParameters>(string.Join("", queryResultChunks));

        }
        public async Task<StoredProcedureModel[]> GetSpListAsync()
        {
            await using var connection = new SqlConnection(_connectionString);

            const string query = "SELECT \n\t\t\'StoredProcedureName\'=name,\n\t\t\'StoredProcedureObjectId\'=object_id\n\tFROM [sys].[procedures] sp\n\tWHERE is_ms_shipped = 0\n\tAND NOT EXISTS (\n\t\tselect ep.[major_id]\n\t\tfrom [sys].[extended_properties] ep\n\t\twhere ep.[major_id] = sp.[object_id]\n\t\tand ep.[minor_id] = 0\n\t\tand ep.[class] = 1\n\t\tand ep.[name] = N\'microsoft_database_tools_support\')\n\t\tFOR JSON PATH;";
            
            var queryResultChunks = await connection.QueryAsync<string>(query,
            commandType: CommandType.Text);

            return JsonConvert.DeserializeObject<StoredProcedureModel[]>(string.Join("", queryResultChunks));
        }
    }
}