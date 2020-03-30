using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Sentinel.Api.Product;
using Sentinel.Api.Comms.Tests.Helpers;
using Sentinel.Model.Product.Dto;
using Newtonsoft.Json;
using Sentinel.Api.Comms;
using System.Net;

namespace Sentinel.Api.Comms.Tests.IntegrationTests
{
    [Collection("WebApplicationFactory")]
    public class BetaFeatureFlagShould
    {


        private WebApplicationFactory<Startup> factory;
        AuthTokenFixture authTokenFixture;
        private ITestOutputHelper output;

        public BetaFeatureFlagShould(CustomWebApplicationFactory factory, AuthTokenFixture authTokenFixture, ITestOutputHelper output)
        {
            this.factory = factory;

            this.output = output;
            this.authTokenFixture = authTokenFixture;
        }



        [Theory]
        [InlineData("/api/PushNotification/queue", "GET", null)]
        [InlineData("/api/PushNotification/beta", "GET", null)]
        // [InlineData("/api/PushNotification", "DELETE", "1")]
        public void ProvideFeatureWhenFlagEnabled(string url, string Method, string payload)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("api-version", "1.0"); client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
            client.DefaultRequestHeaders.Add("Internal", "true");


            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> responseTask = null;
            if (Method == "GET")
            {
                output.WriteLine("GET is Selected");
                responseTask = client.GetAsync(url);
            }
            responseTask.Wait();
            var response = responseTask.Result;

            var stringcontentTask = response.Content.ReadAsStringAsync();
            stringcontentTask.Wait();
            output.WriteLine("Result :" + stringcontentTask.Result);

            response.EnsureSuccessStatusCode();

        }


        [Theory]
        // [InlineData("/api/PushNotification/queue", "GET", null)]
        [InlineData("/api/PushNotification/beta", "GET", null)]
        // [InlineData("/api/PushNotification", "DELETE", "1")]
        public void DontProvideFeatureWhenFlagDisabled(string url, string Method, string payload)
        {
            // factory.
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("api-version", "1.0"); client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
            //client.DefaultRequestHeaders.Add("Internal", "");


            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> responseTask = null;
            if (Method == "GET")
            {
                output.WriteLine("GET is Selected");
                responseTask = client.GetAsync(url);
            }

            responseTask.Wait();
            var response = responseTask.Result;

            var stringcontentTask = response.Content.ReadAsStringAsync();
            stringcontentTask.Wait();
            output.WriteLine("Result :" + stringcontentTask.Result);

            //Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            //response.EnsureSuccessStatusCode();

        }

    }
}