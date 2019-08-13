using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Sentinel.Api.HealthMonitoring.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Api.HealthMonitoring.Tests.IntegrationTests
{
    [Collection("WebApplicationFactory")]
    public class HealthCheckDownloaderControllerShould
    {

        private CustomWebApplicationFactory factory;

        //private WebApplicationFactory<Startup> factory;
        //AuthTokenFixture authTokenFixture;
        private ITestOutputHelper output;

        public HealthCheckDownloaderControllerShould(CustomWebApplicationFactory factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
            //  this.authTokenFixture=authTokenFixture;
            //  output.WriteLine("Token Received "+  this.authTokenFixture.Token);
        }

        [Theory]
        [InlineData("https://product.myrcan.com/health/isaliveandwell")]
        public void GetReportforAPIs(string url)
        {
            var fullurl = "api/HealthCheckDownloaderController?url=" + Uri.UnescapeDataString(url);
            var client = factory.CreateClient();
            // client.DefaultRequestHeaders.Add("api-version", "2.0");
            // // client.DefaultRequestHeaders.Add("Accept", "text/plain, application/json, text/json");
            // //client.DefaultRequestHeaders.Add("Accept", "application/json, text/json");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // Act
            client.Timeout = TimeSpan.FromMinutes(5);
            var responseTask = client.GetAsync(fullurl);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // Assert.Equal("application/json; charset=utf-8",
            //     response.Content.Headers.ContentType.ToString());
            var readstrTask = response.Content.ReadAsStringAsync();
            readstrTask.Wait();
            output.WriteLine(readstrTask.Result);
            // Assert.True(false);


        }


        [Theory]
        [InlineData("blah")]
        [InlineData("")]
        public void FailIfUrlWrong(string url)
        {
            var fullurl = "api/HealthCheckDownloaderController?url=" + Uri.UnescapeDataString(url);
            var client = factory.CreateClient();
            // client.DefaultRequestHeaders.Add("api-version", "2.0");
            // // client.DefaultRequestHeaders.Add("Accept", "text/plain, application/json, text/json");
            // //client.DefaultRequestHeaders.Add("Accept", "application/json, text/json");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // Act
            client.Timeout = TimeSpan.FromMinutes(5);
            var responseTask = client.GetAsync(fullurl);
            responseTask.Wait();
            var response = responseTask.Result;


            Assert.False(response.IsSuccessStatusCode);
            // Assert
            //response.EnsureSuccessStatusCode(); // Status Code 200-299
            // Assert.Equal("application/json; charset=utf-8",
            //     response.Content.Headers.ContentType.ToString());
            // var readstrTask = response.Content.ReadAsStringAsync();
            // readstrTask.Wait();
            // output.WriteLine(readstrTask.Result);
            // Assert.True(false);


        }
    }
}