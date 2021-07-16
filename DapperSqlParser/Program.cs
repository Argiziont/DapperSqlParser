using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DapperSqlParser.Models;
using DapperSqlParser.StoredProcedureCodeGeneration;

namespace DapperSqlParser
{
    internal static class Program
    {
        private const string ConnectionString =
            @"Server= ARGIZIONT-PC\SQLExpress;Database=ShopParserDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private const string NameSpaceName = "DapperSqlParser.TestRepository.Service.GeneratedClientFile";

        private static async Task Main(string[] args)
        {
            StoredProcedureService spService = new StoredProcedureService(ConnectionString);
            List<StoredProcedureParameters> paramsList;

            #region argsLogic

            if (!args.Any())
                Console.WriteLine("Specify stored procedures for parsing: \n" +
                                  "\t-all :for all procedures\n" +
                                  "\t-mod [sp1],[sp2],[sp3],[...] :for procedures with given names");

            if (args.Contains("-all"))
                paramsList = await spService.GenerateModelsListAsync();
            else if (args.Contains("-mod"))
                paramsList = await spService.GenerateModelsListAsync(args[1].Split(','));
            else
                return;

            #endregion

            StoredProceduresCodeGenerator storedProcedureCodeGenerator = new StoredProceduresCodeGenerator(new StoredProcedureParseBuilder(new StoredProceduresDataModelExtractor()))
                { NameSpaceName = NameSpaceName, Parameters = paramsList };

            string storedProcedureGeneratedCode = await storedProcedureCodeGenerator.CreateSpClient();

            await WriteGeneratedCodeToClientFile(storedProcedureGeneratedCode);
        }

        private static async Task WriteGeneratedCodeToClientFile(string generatedCode)
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            string filePath = Path.Combine(projectPath ?? throw new InvalidOperationException(),
                @"GeneratedFile\spClient.cs");

            if (generatedCode == null) throw new ArgumentNullException(nameof(generatedCode));
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));


            await File.WriteAllTextAsync(filePath, generatedCode);
        }
    }
}
