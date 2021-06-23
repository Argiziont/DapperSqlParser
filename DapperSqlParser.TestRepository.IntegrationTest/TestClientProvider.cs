using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace DapperSqlParser.TestRepository.IntegrationTest
{
    public class TestClientProvider
    {
        public HttpClient Client { get; }
       

        public TestClientProvider()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            var server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile(configPath);
                })
                .UseStartup<Startup>());

            Client = server.CreateClient();
        }

    }
}
