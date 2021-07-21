using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace DapperSqlParser.TestRepository.IntegrationTest
{
    public class TestClientProvider
    {
        public TestClientProvider()
        {
            string projectDir = Directory.GetCurrentDirectory();
            string configPath = Path.Combine(projectDir, "appsettings.json");

            TestServer server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, conf) => { conf.AddJsonFile(configPath); })
                .UseStartup<Startup>());

            Client = server.CreateClient();
        }

        public HttpClient Client { get; }
    }
}