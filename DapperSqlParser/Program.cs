using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DapperSqlParser.Models;
using DapperSqlParser.Services;


namespace DapperSqlParser
{
    internal static class Program
    {
        private const string ConnectionString =
            @"Server= ARGIZIONT-PC\SQLExpress;Database=ShopParserDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private const string NameSpaceName = "DapperSqlParser.TestRepository.Service.GeneratedClientFile";

        private static async Task Main(string[] args)
        {
            if (!args.Any())
                Console.WriteLine("Specify stored procedures for parsing: \n" +
                                  "\t-all :for all procedures\n" +
                                  "\t-mod [sp1],[sp2],[sp3],[...] :for procedures with given names");

            var spService = new StoredProcedureService(ConnectionString);
            List<StoredProcedureParameters> paramsList;

            if (args.Contains("-all"))
                paramsList = await spService.GenerateModelsListAsync();
            else if (args.Contains("-mod"))
                paramsList = await spService.GenerateModelsListAsync(args[1].Split(','));
            else
                return;

            var spNamespace = await StoredProceduresExtractor.CreateSpClient(paramsList, NameSpaceName);


            // This will get the current PROJECT directory
            var projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            var filePath = Path.Combine(projectPath ?? throw new InvalidOperationException(),
                @"GeneratedFile\spClient.cs");

            await StoredProceduresExtractor.WriteGeneratedNamespaceToClientFile(spNamespace, filePath);
        }
    }
}