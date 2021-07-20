using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;
using DapperSqlParser.StoredProcedureCodeGeneration.StoredProcedureParsers;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public class StoredProcedureParseBuilder: IStoredProcedureParseBuilder
    {
        private StringBuilder _internalStringBuilder;

        public void SetStringBuilder(StringBuilder stringBuilder)
        {
            _internalStringBuilder = stringBuilder ?? throw new ArgumentNullException(nameof(stringBuilder));
        }
        
        public void AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo)
        {
            if (storedProcedureInfo == null) throw new ArgumentNullException(nameof(storedProcedureInfo));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            _internalStringBuilder.AppendLine(CodeGeneratorUtils.CreateStoredProcedureErrorComment(storedProcedureInfo.Name, storedProcedureInfo.Error));
        }

        public void AppendStoredProcedureNotFoundMessage(StoredProcedureInfo storedProcedureInfo)
        {
            if (storedProcedureInfo == null) throw new ArgumentNullException(nameof(storedProcedureInfo));
            if (storedProcedureInfo.Name == null) throw new ArgumentNullException(nameof(storedProcedureInfo.Name));

            _internalStringBuilder.AppendLine(CodeGeneratorUtils.CreateStoredProcedureNotFoundComment(storedProcedureInfo.Name));
        }

        
        
        public async Task AppendExtractedCsSharpCode(StoredProcedureParameters spParameter)
        {
            if (spParameter == null) throw new ArgumentNullException(nameof(spParameter));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            StoredProcedureInputModelGenerator storedProcedureInputModelGenerator =
                new StoredProcedureInputModelGenerator(spParameter.InputParametersDataModels, spParameter.StoredProcedureInfo.Name, spParameter.StoredProcedureText.Definition);

            StoredProcedureOutputModelGenerator storedProcedureOutputModelGenerator =
                new StoredProcedureOutputModelGenerator(spParameter.OutputParametersDataModels, spParameter.StoredProcedureInfo.Name, spParameter.StoredProcedureText.Definition);

            StoredProcedureClientClassGenerator storedProcedureClientClassGenerator =
                new StoredProcedureClientClassGenerator(spParameter.InputParametersDataModels,
                    spParameter.OutputParametersDataModels, spParameter.StoredProcedureInfo.Name,
                    StoreProcedureInputIsJson(spParameter.OutputParametersDataModels?.First().ParameterName));

            var storedProcedureBuilder = new StringBuilder();


            string outputModelClass =
                await storedProcedureOutputModelGenerator.GenerateAsync();
            _internalStringBuilder.AppendLine(outputModelClass);

            string inputModelClass =
                await storedProcedureInputModelGenerator.GenerateAsync();
            _internalStringBuilder.AppendLine(inputModelClass);

            string clientClass =
                await storedProcedureClientClassGenerator.GenerateAsync();
            _internalStringBuilder.AppendLine(clientClass);

        }


        private bool StoreProcedureInputIsJson(string inputParameterName)
        {
            return inputParameterName != null && Guid.TryParse(inputParameterName.Replace("JSON_", ""), out _);
        }
    }
}