using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Api.Comms.Tests.Controllers
{
    public class HomeServerTests : IClassFixture<WebApplicationFactory<Sentinel.Api.Comms.Startup>>
    {
        private WebApplicationFactory<Startup> factory;

        public HomeServerTests(WebApplicationFactory<Sentinel.Api.Comms.Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/About")]
        [InlineData("/Home/Privacy")]
        [InlineData("/Home/Contact")]
        public void Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var responseTask = client.GetAsync(url);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}