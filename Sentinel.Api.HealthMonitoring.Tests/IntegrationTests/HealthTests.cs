using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Sentinel.Api.HealthMonitoring;
using Sentinel.Api.HealthMonitoring.Tests.Helpers;
using System.Net;

namespace Sentinel.Api.HealthMonitoring.Tests.IntegrationTests
{

    [Collection("WebApplicationFactory")]
    public class HealthTests
    {
        private CustomWebApplicationFactory factory;
        AuthTokenFixture authTokenFixture;
        private ITestOutputHelper output;

        public HealthTests(CustomWebApplicationFactory factory, AuthTokenFixture authTokenFixture, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
            this.authTokenFixture = authTokenFixture;
            output.WriteLine("Token Received " + this.authTokenFixture.Token);
        }


        [Theory]
        [InlineData("api/healthcheckpush")]
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
            client.Timeout = TimeSpan.FromMinutes(5);
            var responseTask = client.GetAsync(url);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // Assert.Equal("application/json; charset=utf-8",
            //     response.Content.Headers.ContentType.ToString());
        }


        [Theory]
        [InlineData("/Health/IsAliveAndWell")]
        public void HealthChecks_WithAuth_Fails(string url)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Act
            client.Timeout = TimeSpan.FromMinutes(5);
            var responseTask = client.GetAsync(url);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Theory]
        [InlineData("/Health/IsAliveAndWell")]
        public void Health_ChecksWithoutAuth(string url)
        {
            var client = factory.CreateClient();
            // client.DefaultRequestHeaders.Add("api-version", "2.0");
            // // client.DefaultRequestHeaders.Add("Accept", "text/plain, application/json, text/json");
            // //client.DefaultRequestHeaders.Add("Accept", "application/json, text/json");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
            // Act
            client.Timeout = TimeSpan.FromMinutes(5);
            var responseTask = client.GetAsync(url);
            responseTask.Wait();
            var response = responseTask.Result;
            // Assert
            var result = response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.OK;

            // Assert.True(result);
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // Assert.Equal("application/json; charset=utf-8",
            //     response.Content.Headers.ContentType.ToString());
        }

    }
}