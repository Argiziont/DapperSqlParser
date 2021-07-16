using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperSqlParser.Models;
using DapperSqlParser.Services.Exceptions;

namespace DapperSqlParser.Services
{
    public static class StoredProceduresCodeGenerator
    {
        public static async Task<string> CreateSpClientClass(StoredProcedureParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            StringBuilder outputClass = new StringBuilder();

            bool spReturnJsonFlag =
                StoreProcedureInputIsJson(parameters.OutputParametersDataModels?.First().ParameterName);

            outputClass.AppendLine($"\tpublic class {parameters.StoredProcedureInfo.Name} \n\t{{"); //Class name

            StoredProcedureParseBuilder.AppendIDapperExecutorField(parameters, outputClass);
            StoredProcedureParseBuilder.AppendClientConstructor(parameters, outputClass);
            StoredProcedureParseBuilder.AppendExecutorMethod(parameters, outputClass, spReturnJsonFlag);

            outputClass.Append("\t}\n");
            return await Task.FromResult(outputClass.ToString());
        }
        
        public static async Task<string> CreateSpClient(List<StoredProcedureParameters> parameters,
            string namespaceName, IProgress<StoreProcedureGenerationProgress> progress = default)
        {
            StringBuilder outputNamespace = new StringBuilder();
            outputNamespace.AppendLine($"namespace {namespaceName} \n{{");

            foreach (StoredProcedureParameters spParameter in parameters)
            {
                ReportAboutStoredProcedureParsingProgress(parameters, progress, spParameter);

                outputNamespace.AppendLine(
                    $"\n\t#region {spParameter.StoredProcedureInfo.Name}"); //Wrapping every sp into region

                try
                {
                    if (spParameter.StoredProcedureInfo.Error != null)
                    {
                        outputNamespace.AppendLine("//Couldn't parse Stored procedure  with name: " +
                                                   $"{spParameter.StoredProcedureInfo.Name} because of internal error: " +
                                                   $"{spParameter.StoredProcedureInfo.Error}\n\t#endregion");
                        continue;
                    }

                    await StoredProcedureParseBuilder.AppendExtractedCsSharpCode(spParameter, outputNamespace);

                    //await Task.Delay(200); //Await 200ms for progress bar testing, could be deleted if not needed
                }
                catch (NullModelException)
                {
                    outputNamespace.AppendLine(
                        $"//Model for {spParameter.StoredProcedureInfo.Name} was not found, could not parse this Stored Procedure!");
                }

                outputNamespace.AppendLine("\t#endregion");
            }

            outputNamespace.Append("}");
            return await Task.FromResult(outputNamespace.ToString());
        }

        public static async Task WriteGeneratedCodeToClientFile(string generatedCode, string filePath)
        {
            if (generatedCode == null) throw new ArgumentNullException(nameof(generatedCode));
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));


            await File.WriteAllTextAsync(filePath, generatedCode);
        }
       
        private static bool StoreProcedureInputIsJson(string inputParameterName)
        {
            return inputParameterName != null && Guid.TryParse(inputParameterName.Replace("JSON_", ""), out _);
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