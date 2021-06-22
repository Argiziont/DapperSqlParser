using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DapperSqlParser.TestRepository.IntegrationTest
{
    public class CategoriesIntegrationTest
    {
        private const string ControllerApiPath = "/api/Categories";

        [Theory]
        [InlineData(ControllerApiPath + "/GetAll")]
        public async Task GetWithoutParameters_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            //Act
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData(ControllerApiPath + "/GetAll", 2)]
        [InlineData(ControllerApiPath + "/GetAll", 20)]
        [InlineData(ControllerApiPath + "/GetAll", 30)]
        public async Task GetWithParameters_EndpointsReturnSuccessAndCorrectContentType(string url, int categoryId)
        {
            //Arrange
            var client = new TestClientProvider().Client;
            //Act
            var response = await client.GetAsync($"{url}?categoryId={categoryId}");
            response.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

    }
}