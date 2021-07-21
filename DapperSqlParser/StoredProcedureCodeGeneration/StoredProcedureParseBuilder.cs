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
        
        public async Task AppendStoredProcedureCantParseMessage(StoredProcedureInfo storedProcedureInfo)
        {
            if (storedProcedureInfo == null) throw new ArgumentNullException(nameof(storedProcedureInfo));
            if (_internalStringBuilder == null) throw new ArgumentNullException(nameof(_internalStringBuilder));

            StoredProcedureRegionGenerator storedProcedureRegionGenerator =
                new StoredProcedureRegionGenerator(storedProcedureInfo.Name,
                    CodeGeneratorUtils.CreateStoredProcedureErrorComment(storedProcedureInfo.Name, storedProcedureInfo.Error));

            _internalStringBuilder.AppendLine(await storedProcedureRegionGenerator.GenerateAsync());
        }

        public async Task AppendStoredProcedureNotFoundMessage(StoredProcedureInfo storedProcedureInfo)
        {
            if (storedProcedureInfo == null) throw new ArgumentNullException(nameof(storedProcedureInfo));
            if (storedProcedureInfo.Name == null) throw new ArgumentNullException(nameof(storedProcedureInfo.Name));

            StoredProcedureRegionGenerator storedProcedureRegionGenerator =
                new StoredProcedureRegionGenerator(storedProcedureInfo.Name,
                    CodeGeneratorUtils.CreateStoredProcedureNotFoundComment(storedProcedureInfo.Name));

            _internalStringBuilder.AppendLine(await storedProcedureRegionGenerator.GenerateAsync());

        }

        public async Task AppendExtractedCsSharpCode(StoredProcedureParameters storedProcedureParameters)
        {
            if (storedProcedureParameters == null) 
                throw new ArgumentNullException(nameof(storedProcedureParameters));

            StoredProcedureInputModelGenerator storedProcedureInputModelGenerator =
                new StoredProcedureInputModelGenerator(storedProcedureParameters.InputParametersDataModels, storedProcedureParameters.StoredProcedureInfo.Name, storedProcedureParameters.StoredProcedureText.Definition);

            StoredProcedureOutputModelGenerator storedProcedureOutputModelGenerator =
                new StoredProcedureOutputModelGenerator(storedProcedureParameters.OutputParametersDataModels, storedProcedureParameters.StoredProcedureInfo.Name, storedProcedureParameters.StoredProcedureText.Definition);

            StoredProcedureClientClassGenerator storedProcedureClientClassGenerator =
                new StoredProcedureClientClassGenerator(storedProcedureParameters.InputParametersDataModels,
                    storedProcedureParameters.OutputParametersDataModels, storedProcedureParameters.StoredProcedureInfo.Name,
                    StoreProcedureInputIsJson(storedProcedureParameters.OutputParametersDataModels?.First().ParameterName));

            StringBuilder storedProcedureLogicBuilder = new StringBuilder();


            storedProcedureLogicBuilder.AppendLine(await storedProcedureOutputModelGenerator.GenerateAsync());
            storedProcedureLogicBuilder.AppendLine(await storedProcedureInputModelGenerator.GenerateAsync());
            storedProcedureLogicBuilder.AppendLine(await storedProcedureClientClassGenerator.GenerateAsync());


            StoredProcedureRegionGenerator storedProcedureRegionGenerator =
                new StoredProcedureRegionGenerator(storedProcedureParameters.StoredProcedureInfo.Name,
                    storedProcedureLogicBuilder.ToString());

            _internalStringBuilder.AppendLine(await storedProcedureRegionGenerator.GenerateAsync());
        }

        private bool StoreProcedureInputIsJson(string inputParameterName)
        {
            return inputParameterName != null && Guid.TryParse(inputParameterName.Replace("JSON_", ""), out _);
        }
    }
}