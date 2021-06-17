using System;

namespace DapperSqlParser.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonWrapperAttribute:Attribute
    {
        public string StoreProcedureJsonInputName { get; private set; }

        public JsonWrapperAttribute(string storeProcedureJsonInputName)
        {
            this.StoreProcedureJsonInputName = storeProcedureJsonInputName;
        }
    }
}