using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Sentinel.Api.Product;
using Sentinel.Api.Product.Tests.Helpers;
using Sentinel.Model.Product.Dto;
using Newtonsoft.Json;

namespace Sentinel.Api.Product.Tests.IntegrationTests
{

    [Collection("WebApplicationFactory")]
    public class ProductV1Tests
    {
        private WebApplicationFactory<Startup> factory;
        AuthTokenFixture authTokenFixture;
        private ITestOutputHelper output;

        public ProductV1Tests(CustomWebApplicationFactory factory, AuthTokenFixture authTokenFixture, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
            this.authTokenFixture = authTokenFixture;
            //  output.WriteLine("Token Received "+  this.authTokenFixture.Token);
        }


        [Theory]
        [InlineData("/api/Product", "GET", null)]
        [InlineData("/api/Product", "POST", "{}")]
        [InlineData("/api/Product", "PUT", "{}")]
        [InlineData("/api/Product", "DELETE", "1")]
        public void ProductV1_EndpointsReturnSuccessAndCorrectContentType(string url, string Method, string payload)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
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

                Random rnd = new Random();

                ProductInfoDtoV1 newproduct = new ProductInfoDtoV1();
                newproduct.Id = rnd.Next(1000, 100000);
                var stringPayload = JsonConvert.SerializeObject(newproduct);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
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
            // Assert.Equal("application/json; charset=utf-8",
            //     response.Content.Headers.ContentType.ToString());
        }



    }
}