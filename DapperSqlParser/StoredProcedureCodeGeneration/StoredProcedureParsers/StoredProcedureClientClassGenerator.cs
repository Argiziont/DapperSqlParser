using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Extensions;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;
using static DapperSqlParser.StoredProcedureCodeGeneration.TemplateService.JsonNamingConstants;

namespace DapperSqlParser.StoredProcedureCodeGeneration.StoredProcedureParsers
{
    public class StoredProcedureClientClassGenerator : ICodeGenerator
    {
        private readonly object _inputParameter;
        private readonly object _outputParameter;
        private readonly string _storedProcedureName;
        private readonly bool _isReturnTypeJson;

        public StoredProcedureClientClassGenerator(object inputParameter, object outputParameter, string storedProcedureName, bool isReturnTypeJson)
        {
            _inputParameter = inputParameter;
            _outputParameter = outputParameter;
            _storedProcedureName = storedProcedureName;
            _isReturnTypeJson = isReturnTypeJson;
        }

        public async Task<string> GenerateAsync()
        {
           return await Task.FromResult(CreateClientClass());
        }

        private string CreateClientClass()
        {
            StringBuilder clientClass = new StringBuilder();

            clientClass.AppendLine( CodeGeneratorUtils.CreateIDapperExecutorField(_inputParameter, _outputParameter, _storedProcedureName));
            clientClass.AppendLine(CodeGeneratorUtils.CreateDapperClientConstructor(_inputParameter, _outputParameter, _storedProcedureName));
            clientClass.AppendLine(CodeGeneratorUtils.CreateDapperClientMethod(_inputParameter, _outputParameter, _storedProcedureName, _isReturnTypeJson));

            return CodeGeneratorUtils.CreateClass(_storedProcedureName, clientClass.ToString());
        }

    }
}