using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;
using DapperSqlParser.Services.Exceptions;

namespace DapperSqlParser.Services
{
    public static class StoredProceduresCodeGenerator
    {
        public static async Task<string> CreateSpClient(List<StoredProcedureParameters> parameters,
            string namespaceName, IProgress<StoreProcedureGenerationProgress> progress = default)
        {
            StringBuilder outputCode = new StringBuilder();

            outputCode.AppendLine($"namespace {namespaceName} \n{{"); // Append name space start

            foreach (StoredProcedureParameters spParameter in parameters)
            {
                ReportAboutStoredProcedureParsingProgress(parameters, progress, spParameter);

                StoredProcedureParseBuilder.AppendStoredProcedureRegionStart(spParameter.StoredProcedureInfo.Name,
                    outputCode);

                try
                {
                    if (spParameter.StoredProcedureInfo.Error != null)
                    {
                        StoredProcedureParseBuilder.AppendStoredProcedureCantParseMessage(
                            spParameter.StoredProcedureInfo, outputCode);
                        continue;
                    }

                    await StoredProcedureParseBuilder.AppendExtractedCsSharpCode(spParameter, outputCode);

                    //await Task.Delay(200); //Await 200ms for progress bar testing, could be deleted if not needed
                }
                catch (NullModelException)
                {
                    StoredProcedureParseBuilder.AppendStoredProcedureNotFoundMessage(
                        spParameter.StoredProcedureInfo.Name, outputCode);
                }

                StoredProcedureParseBuilder.AppendStoredProcedureRegionEnd(outputCode);
            }

            outputCode.Append("}"); // Append name space end

            return await Task.FromResult(outputCode.ToString());
        }

        private static void ReportAboutStoredProcedureParsingProgress(IList<StoredProcedureParameters> parameters,
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