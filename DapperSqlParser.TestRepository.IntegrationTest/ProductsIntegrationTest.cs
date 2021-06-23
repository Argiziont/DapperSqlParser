using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DapperSqlParser.TestRepository.IntegrationTest
{
    public class ProductsIntegrationTest
    {
        private const string ControllerApiPath = "/api/Products";

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
        [InlineData(ControllerApiPath + "/GetById", 2)]
        [InlineData(ControllerApiPath + "/GetById", 6)]
        [InlineData(ControllerApiPath + "/GetById", 8)]
        public async Task GetWithParameters_EndpointsReturnSuccessAndCorrectContentType(string url, int productId)
        {
            //Arrange
            var client = new TestClientProvider().Client;

            //Act
            var response = await client.GetAsync($"{url}?productId={productId}");
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData(ControllerApiPath + "/DeleteById", 10)]
        [InlineData(ControllerApiPath + "/DeleteById", 20)]
        [InlineData(ControllerApiPath + "/DeleteById", 30)]
        public async Task Delete_EndpointsReturnSuccessAndCorrectContentType(string url, int productId)
        {
            //Restore
            await DatabaseExtensions.RestoreDatabase();

            //Arrange
            var client = new TestClientProvider().Client;

            //Act
            var response = await client.DeleteAsync($"{url}?productId={productId}");
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(ControllerApiPath + "/Insert", "{\n  \"id\": 0,\n  \"externalId\": \"Test1\",\n  \"title\": \"Test1\",\n  \"url\": \"Test1\",\n  \"syncDate\": \"2021-06-23T10:07:08.566Z\",\n  \"productState\": 0,\n  \"description\": \"Test1\",\n  \"price\": \"Test1\"\n}")]
        [InlineData(ControllerApiPath + "/Insert", "{\n  \"id\": 0,\n  \"externalId\": \"Test2\",\n  \"title\": \"Test2\",\n  \"url\": \"Test2\",\n  \"syncDate\": \"2021-06-23T10:07:08.566Z\",\n  \"productState\": 0,\n  \"description\": \"Test2\",\n  \"price\": \"Test2\"\n}")]
        [InlineData(ControllerApiPath + "/Insert", "{\n  \"id\": 0,\n  \"externalId\": \"Test3\",\n  \"title\": \"Test3\",\n  \"url\": \"Test3\",\n  \"syncDate\": \"2021-06-23T10:07:08.566Z\",\n  \"productState\": 0,\n  \"description\": \"Test3\",\n  \"price\": \"Test3\"\n}")]
        public async Task Insert_EndpointsReturnSuccessAndCorrectContentType(string url, string jsonPayLoad)
        {
            //Restore
            await DatabaseExtensions.RestoreDatabase();

            //Arrange
            var client = new TestClientProvider().Client;
            var stringContent = new StringContent(jsonPayLoad.ToString(), System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync($"{url}", stringContent);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(ControllerApiPath + "/UpdateTitleById", 10, "\"Test1\"")]
        [InlineData(ControllerApiPath + "/UpdateTitleById", 20, "\"Test2\"")]
        [InlineData(ControllerApiPath + "/UpdateTitleById", 30, "\"Test3\"")]
        public async Task Update_EndpointsReturnSuccessAndCorrectContentType(string url, int productId, string productName)
        {
            //Restore
            await DatabaseExtensions.RestoreDatabase();

            //Arrange
            var client = new TestClientProvider().Client;

            //Act
            var response = await client.PutAsync($"{url}?productId={productId}", new StringContent(productName, System.Text.Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}