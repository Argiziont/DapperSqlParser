﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Exceptions;
using DapperSqlParser.Models;

namespace DapperSqlParser.StoredProcedureCodeGeneration
{
    public class StoredProceduresCodeGenerator
    {
        public string NameSpaceName { get; set; }
        public IList<StoredProcedureParameters> Parameters { get; set; }

        private readonly StoredProcedureParseBuilder _storedProcedureCodeBuilder;

        public StoredProceduresCodeGenerator(StoredProcedureParseBuilder storedProcedureCodeBuilder)
        {
            _storedProcedureCodeBuilder = storedProcedureCodeBuilder;
        }
        public async Task<string> CreateSpClient(IProgress<StoreProcedureGenerationProgress> progress = default)
        {
            if (Parameters == null) throw new ArgumentNullException(nameof(Parameters));
            if (NameSpaceName == null) throw new ArgumentNullException(nameof(NameSpaceName));

            StringBuilder outputCode = new StringBuilder();

            outputCode.AppendLine($"namespace {NameSpaceName} \n{{"); // Append name space start

            _storedProcedureCodeBuilder.SetStringBuilder(outputCode);

            foreach (StoredProcedureParameters spParameter in Parameters)
            {
                ReportAboutStoredProcedureParsingProgress(Parameters, progress, spParameter);

                _storedProcedureCodeBuilder.AppendStoredProcedureRegionStart(spParameter.StoredProcedureInfo.Name);

                try
                {
                    if (spParameter.StoredProcedureInfo.Error != null)
                    {
                        _storedProcedureCodeBuilder.AppendStoredProcedureCantParseMessage(
                            spParameter.StoredProcedureInfo);
                        continue;
                    }

                    await _storedProcedureCodeBuilder.AppendExtractedCsSharpCode(spParameter);

                    //await Task.Delay(200); //Await 200ms for progress bar testing, could be deleted if not needed
                }
                catch (NullModelException)
                {
                    _storedProcedureCodeBuilder.AppendStoredProcedureNotFoundMessage(
                        spParameter.StoredProcedureInfo.Name);
                }

                _storedProcedureCodeBuilder.AppendStoredProcedureRegionEnd();
            }

            outputCode.Append("}"); // Append name space end

            return await Task.FromResult(outputCode.ToString());
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