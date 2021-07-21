using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DapperSqlParser.TestRepository.IntegrationTest
{
    [Collection("Sequential")]
    public class CategoriesIntegrationTest
    {
        private const string ControllerApiPath = "/api/Categories";

        [Theory]
        [InlineData(ControllerApiPath + "/GetAll")]
        public async Task GetWithoutParameters_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            //Arrange
            HttpClient client = new TestClientProvider().Client;

            //Act
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData(ControllerApiPath + "/GetById", 3)]
        [InlineData(ControllerApiPath + "/GetById", 20)]
        [InlineData(ControllerApiPath + "/GetById", 30)]
        public async Task GetWithParameters_EndpointsReturnSuccessAndCorrectContentType(string url, int categoryId)
        {
            //Arrange
            HttpClient client = new TestClientProvider().Client;

            //Act
            HttpResponseMessage response = await client.GetAsync($"{url}?categoryId={categoryId}");
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData(ControllerApiPath + "/DeleteById", 10)]
        [InlineData(ControllerApiPath + "/DeleteById", 20)]
        [InlineData(ControllerApiPath + "/DeleteById", 30)]
        public async Task Delete_EndpointsReturnSuccessAndCorrectContentType(string url, int categoryId)
        {
            //Restore
            await DatabaseExtensions.RestoreDatabase();

            //Arrange
            HttpClient client = new TestClientProvider().Client;

            //Act
            HttpResponseMessage response = await client.DeleteAsync($"{url}?categoryId={categoryId}");
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(ControllerApiPath + "/Insert", "{\"id\":0,\"name\":\"Test1\",\"url\":\"Test1\"}")]
        [InlineData(ControllerApiPath + "/Insert", "{\"id\":0,\"name\":\"Test2\",\"url\":\"Test2\"}")]
        [InlineData(ControllerApiPath + "/Insert", "{\"id\":0,\"name\":\"Test3\",\"url\":\"Test3\"}")]
        public async Task Insert_EndpointsReturnSuccessAndCorrectContentType(string url, string jsonPayLod)
        {
            //Restore
            await DatabaseExtensions.RestoreDatabase();

            //Arrange
            HttpClient client = new TestClientProvider().Client;
            StringContent stringContent = new StringContent(jsonPayLod, Encoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await client.PostAsync($"{url}", stringContent);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(ControllerApiPath + "/UpdateTitleById", 10, "\"Test1\"")]
        [InlineData(ControllerApiPath + "/UpdateTitleById", 20, "\"Test2\"")]
        [InlineData(ControllerApiPath + "/UpdateTitleById", 30, "\"Test3\"")]
        public async Task Update_EndpointsReturnSuccessAndCorrectContentType(string url, int categoryId,
            string categoryTitle)
        {
            //Restore
            await DatabaseExtensions.RestoreDatabase();

            //Arrange
            HttpClient client = new TestClientProvider().Client;

            //Act
            HttpResponseMessage response = await client.PutAsync($"{url}?categoryId={categoryId}",
                new StringContent(categoryTitle, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}