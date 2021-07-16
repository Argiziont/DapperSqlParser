using System;

namespace DapperSqlParser.Dapper_Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonWrapperAttribute : Attribute
    {
        public JsonWrapperAttribute(string storeProcedureJsonInputName)
        {
            StoreProcedureJsonInputName = storeProcedureJsonInputName;
        }

        public string StoreProcedureJsonInputName { get; }
    }
}