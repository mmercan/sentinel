using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Sentinel.Api.HealthMonitoring.Tests.Helpers;

namespace Sentinel.Api.HealthMonitoring.Tests.IntegrationTests
{

    [Collection("WebApplicationFactory")]
    public class HomeTests
    {
        private CustomWebApplicationFactory factory;
        private ITestOutputHelper output;
        private AuthTokenFixture authTokenFixture;

        public HomeTests(CustomWebApplicationFactory factory, AuthTokenFixture authTokenFixture, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
            this.authTokenFixture = authTokenFixture;
        }


        [Theory]
        // [InlineData("/")]
        [InlineData("/Home/Index")]
        // [InlineData("/Home/About")]
        // [InlineData("/Home/Contact")]
        [InlineData("/Home/Error")]
        public void Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
            //factory.WithWebHostBuilder()
            // Act
            var responseTask = client.GetAsync(url);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }


        [InlineData("/Home/Privacy")]
        public void Get_EndpointsReturnSuccessAndCorrectContentTypeForPrivacy(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
            //factory.WithWebHostBuilder()
            // Act
            var responseTask = client.GetAsync(url);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/plain; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }


        // [Theory]
        // // [InlineData("/")]
        // [InlineData("http://localhost:5006/home/index")]
        // public void ExternalOnes(string url)
        // {
        //     HttpClient client = new HttpClient();
        //     client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
        //     var responseTask = client.GetAsync(url);
        //     responseTask.Wait();
        //     var response = responseTask.Result;
        //     // Assert
        //     output.WriteLine(this.authTokenFixture.Token);
        //     response.EnsureSuccessStatusCode(); // Status Code 200-299
        //     Assert.Equal("text/html; charset=utf-8",
        //         response.Content.Headers.ContentType.ToString());
        // }

    }
}