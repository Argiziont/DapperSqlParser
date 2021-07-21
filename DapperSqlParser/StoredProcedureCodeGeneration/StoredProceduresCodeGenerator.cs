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
        public string NameSpaceName { get; set; }
        public IList<StoredProcedureParameters> Parameters { get; set; }

        private readonly IStoredProcedureParseBuilder _storedProcedureCodeBuilder;

        public StoredProceduresCodeGenerator(IStoredProcedureParseBuilder storedProcedureCodeBuilder)
        {
            _storedProcedureCodeBuilder = storedProcedureCodeBuilder;
        }
        public async Task<string> CreateSpClient(IProgress<StoreProcedureGenerationProgress> progress = default)
        {
            if (Parameters == null) throw new ArgumentNullException(nameof(Parameters));
            if (NameSpaceName == null) throw new ArgumentNullException(nameof(NameSpaceName));

            StringBuilder outputCode = new StringBuilder();

            _storedProcedureCodeBuilder.SetStringBuilder(outputCode);

            foreach (StoredProcedureParameters spParameter in Parameters)
            {
                ReportAboutStoredProcedureParsingProgress(Parameters, progress, spParameter);

                try
                {
                    if (spParameter.StoredProcedureInfo.Error != null)
                    {
                       await _storedProcedureCodeBuilder.AppendStoredProcedureCantParseMessage(
                            spParameter.StoredProcedureInfo);

                        continue;
                    }

                    await _storedProcedureCodeBuilder.AppendExtractedCsSharpCode(spParameter);

                    //await Task.Delay(200); //Await 200ms for progress bar testing, could be deleted if not needed
                }
                catch (NullModelException)
                {
                    await _storedProcedureCodeBuilder.AppendStoredProcedureNotFoundMessage(
                        spParameter.StoredProcedureInfo);
                }
            }

            string generatedCode=CodeGeneratorUtils.CreateNamespaceWithName(NameSpaceName, outputCode.ToString());
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