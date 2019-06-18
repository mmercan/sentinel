using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Sentinel.Api.Member;
using Sentinel.Api.Member.Tests;
using Sentinel.Api.Member.Tests.Helpers;

namespace Sentinel.Api.Member.Tests.IntegrationTests
{

    [Collection("WebApplicationFactory")]
    public class HealthTests
    {
        private WebApplicationFactory<Startup> factory;
        AuthTokenFixture authTokenFixture;
        private ITestOutputHelper output;

        public HealthTests(WebApplicationFactory<Startup> factory, AuthTokenFixture authTokenFixture, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
            this.authTokenFixture = authTokenFixture;
            //  output.WriteLine("Token Received "+  this.authTokenFixture.Token);
        }


        [Theory]
        [InlineData("/Health/IsAliveAndWell")]
        [InlineData("/Health/IsAlive")]
        public void Health_Checks(string url)
        {
            var client = factory.CreateClient();
            // client.DefaultRequestHeaders.Add("api-version", "2.0");
            // // client.DefaultRequestHeaders.Add("Accept", "text/plain, application/json, text/json");
            // //client.DefaultRequestHeaders.Add("Accept", "application/json, text/json");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // Act
            var responseTask = client.GetAsync(url);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            // Assert.Equal("application/json; charset=utf-8",
            //     response.Content.Headers.ContentType.ToString());
        }


    }
}