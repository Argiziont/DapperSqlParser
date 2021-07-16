using System;
using System.Collections.Generic;
using System.Linq;

namespace DapperSqlParser.Services
{
    public static class SqlCsSharpTypesConverter
    {
        private static readonly Dictionary<string, string> SqlServerTypesTocSharpTypes = new Dictionary<string, string>
        {
            {"bigint", typeof(long).FullName},
            {"binary", typeof(byte[]).FullName},
            {"bit", typeof(bool).FullName},
            {"char", typeof(char).FullName},
            {"date", typeof(DateTime).FullName},
            {"datetime", typeof(DateTime).FullName},
            {"datetime2", typeof(DateTime).FullName},
            {"datetimeoffset", typeof(DateTimeOffset).FullName},
            {"decimal", typeof(decimal).FullName},
            {"filestream", typeof(byte[]).FullName},
            {"float", typeof(double).FullName},
            {"geography", "Microsoft.SqlServer.Types.SqlGeography"},
            {"geometry", "Microsoft.SqlServer.Types.SqlGeometry"},
            {"hierarchyid", "Microsoft.SqlServer.Types.SqlHierarchyId"},
            {"image", typeof(byte[]).FullName},
            {"int", typeof(int).FullName},
            {"money", typeof(decimal).FullName},
            {"nchar", typeof(string).FullName},
            {"ntext", typeof(string).FullName},
            {"numeric", typeof(decimal).FullName},
            {"nvarchar", typeof(string).FullName},
            {"real", typeof(float).FullName},
            {"rowversion", typeof(byte[]).FullName},
            {"smalldatetime", typeof(DateTime).FullName},
            {"smallint", typeof(short).FullName},
            {"smallmoney", typeof(decimal).FullName},
            {"sql_variant", typeof(object).FullName},
            {"text", typeof(string).FullName},
            {"time", typeof(TimeSpan).FullName},
            {"timestamp", typeof(byte[]).FullName},
            {"tinyint", typeof(byte).FullName},
            {"uniqueidentifier", typeof(Guid).FullName},
            {"varbinary", typeof(byte[]).FullName},
            {"varchar", typeof(string).FullName},
            {"xml", typeof(string).FullName}
        };

        public static string ConvertSqlServerFormatToCSharp(string typeName)
        {
            return SqlServerTypesTocSharpTypes.ContainsKey(typeName)
                ? SqlServerTypesTocSharpTypes[typeName]
                : "System.Object";
        }

        public static string ConvertCSharpToSqlServerFormat(string typeName)
        {
            return SqlServerTypesTocSharpTypes.ContainsValue(typeName)
                ? SqlServerTypesTocSharpTypes.FirstOrDefault(x => x.Value == typeName).Key
                : "User-Defined type";
        }
    }
}