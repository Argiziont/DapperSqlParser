using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration.Interfaces;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public class StoredProceduresCodeGenerator
    {
        private readonly string _nameSpaceName;
        private readonly IList<StoredProcedureParameters> _parameters;

        public StoredProceduresCodeGenerator(IList<StoredProcedureParameters> parameters, string nameSpaceName)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            _nameSpaceName = nameSpaceName ?? throw new ArgumentNullException(nameof(nameSpaceName));
        }

        public async Task<string> CreateSpClient(IProgress<StoreProcedureGenerationProgress> progress = default)
        {
            StringBuilder outputCode = new StringBuilder();

            IStoredProcedureParseBuilder storedProcedureParseBuilder = new StoredProcedureParseBuilder(outputCode);

            foreach (StoredProcedureParameters spParameter in _parameters)
            {
                ReportAboutStoredProcedureParsingProgress(_parameters, progress, spParameter);

                try
                {
                    if (spParameter.StoredProcedureInfo.Error != null)
                    {
                        await storedProcedureParseBuilder.AppendStoredProcedureCantParseMessage(
                            spParameter.StoredProcedureInfo);

                        continue;
                    }

                    await storedProcedureParseBuilder.AppendExtractedCsSharpCode(spParameter);

                    //await Task.Delay(200); //Await 200ms for progress bar testing, could be deleted if not needed
                }
                catch (NullModelException)
                {
                    await storedProcedureParseBuilder.AppendStoredProcedureNotFoundMessage(
                        spParameter.StoredProcedureInfo);
                }
            }

            string generatedCode = CodeGeneratorUtils.CreateNamespaceWithName(_nameSpaceName, outputCode.ToString());

            return await Task.FromResult(generatedCode);
        }

        private void ReportAboutStoredProcedureParsingProgress(IList<StoredProcedureParameters> parameters,
            IProgress<StoreProcedureGenerationProgress> progress,
            StoredProcedureParameters spParameter)
        {
            int totalProgressAmount = parameters.Count - 1;
            progress?.Report(new StoreProcedureGenerationProgress
            {
                CurrentProgressAmount = parameters.IndexOf(spParameter),
                TotalProgressAmount = totalProgressAmount,
                CurrentProgressMessage =
                    $"On {parameters.IndexOf(spParameter)} Message"
            });
        }
    }
}