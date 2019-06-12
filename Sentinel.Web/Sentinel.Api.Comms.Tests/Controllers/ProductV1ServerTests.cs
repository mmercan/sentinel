using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Api.Comms.Tests.Controllers
{
    public class ProductV1ServerTests : IClassFixture<WebApplicationFactory<Sentinel.Api.Comms.Startup>>
    {
        private WebApplicationFactory<Startup> factory;
        private readonly ITestOutputHelper output;

        public ProductV1ServerTests(WebApplicationFactory<Sentinel.Api.Comms.Startup> factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
            // postobject 
        }

        [Theory]

        [InlineData("/api/Product", "GET", null)]
        [InlineData("/api/Product", "POST", "{}")]
        [InlineData("/api/Product", "PUT", "{}")]
        [InlineData("/api/Product", "DELETE", "1")]
        public void Get_EndpointsReturnSuccessAndCorrectContentType(string url, string Method, string payload)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            // client.DefaultRequestHeaders.Add("Accept", "text/plain, application/json, text/json");
            //client.DefaultRequestHeaders.Add("Accept", "application/json, text/json");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // Act
            System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> responseTask = null;
            if (Method == "GET")
            {
                output.WriteLine("GET is Selected");
                responseTask = client.GetAsync(url);
            }
            else if (Method == "POST")
            {
                output.WriteLine("POST is Selected");
                //  var stringPayload =   JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                responseTask = client.PostAsync(url, httpContent);
            }
            else if (Method == "PUT")
            {
                output.WriteLine("PUT is Selected");
                var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                responseTask = client.PutAsync(url, httpContent);
            }
            else if (Method == "DELETE")
            {
                output.WriteLine("DELETE is Selected");
                //var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                url = url + "?id=" + payload;
                responseTask = client.DeleteAsync(url);
            }
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}