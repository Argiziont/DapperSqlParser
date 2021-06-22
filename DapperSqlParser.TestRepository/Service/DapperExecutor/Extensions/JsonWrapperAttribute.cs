using System;

namespace DapperSqlParser.TestRepository.Service.DapperExecutor.Extensions
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