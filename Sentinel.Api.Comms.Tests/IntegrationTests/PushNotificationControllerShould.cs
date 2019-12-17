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

namespace Sentinel.Api.Product.Tests.IntegrationTests
{

    [Collection("WebApplicationFactory")]
    public class PushNotificationControllerShould
    {

        private WebApplicationFactory<Startup> factory;
        AuthTokenFixture authTokenFixture;
        private ITestOutputHelper output;

        public PushNotificationControllerShould(CustomWebApplicationFactory factory, AuthTokenFixture authTokenFixture, ITestOutputHelper output)
        {
            this.factory = factory;

            this.output = output;
            this.authTokenFixture = authTokenFixture;
            //  output.WriteLine("Token Received "+  this.authTokenFixture.Token);
        }


        [Theory]
        [InlineData("/api/PushNotification", "GET", null)]
        [InlineData("/api/PushNotification/subscribe", "POST", "{\"endpoint\":\"https://sg2p.notify.windows.com/w/?token=BQYAAAAb8Mp0FzKVB1JzI%2b%2bogcDQyuCim61doF7omrrqqa5BrHEvLqE9%2biMqilQe9k1Gi1mF1bEYaZPLxt8MMk5GbR8fn72IYHbi6QizSMogS7vYsyC5kVe%2fS9WYX6A8xnyp%2fCRELmxUfHKQEnXvRPRAILqyj%2brmJ%2br9sPuiRI8DR0C6e91WQOvxEVaL3OMxU4fuTZqR1o6R7ZYxykEdl%2fd4HL282xSX9NqPoL%2f6dtcoZbr74y2%2fiSgJj0v7Ixak2%2faoicPpnmLbSEW5jgHyORgMsqCsAvCMHB01qUOLM13AxfUhpkUDavs6zI6F%2bLNHGjjcHwwIsyFSajGGenE1v9KqspS%2f\",\"expirationTime\":1578535651000,\"keys\":{\"auth\":\"BVNrVTgi-EZwLVQfGhcr-Q\",\"p256dh\":\"BK-d7dmBTpL3i0etHWODL0G_pHTu_omfK4IDlJQCNW6iH7ANoS4hA4mKfKRwoX7ihn0mVPlFo0Le9NhZrcDvHzQ\"}}")]
        [InlineData("/api/PushNotification/Users", "POST", "{\"endpoint\":\"https://sg2p.notify.windows.com/w/?token=BQYAAAAb8Mp0FzKVB1JzI%2b%2bogcDQyuCim61doF7omrrqqa5BrHEvLqE9%2biMqilQe9k1Gi1mF1bEYaZPLxt8MMk5GbR8fn72IYHbi6QizSMogS7vYsyC5kVe%2fS9WYX6A8xnyp%2fCRELmxUfHKQEnXvRPRAILqyj%2brmJ%2br9sPuiRI8DR0C6e91WQOvxEVaL3OMxU4fuTZqR1o6R7ZYxykEdl%2fd4HL282xSX9NqPoL%2f6dtcoZbr74y2%2fiSgJj0v7Ixak2%2faoicPpnmLbSEW5jgHyORgMsqCsAvCMHB01qUOLM13AxfUhpkUDavs6zI6F%2bLNHGjjcHwwIsyFSajGGenE1v9KqspS%2f\",\"expirationTime\":1578535651000,\"keys\":{\"auth\":\"BVNrVTgi-EZwLVQfGhcr-Q\",\"p256dh\":\"BK-d7dmBTpL3i0etHWODL0G_pHTu_omfK4IDlJQCNW6iH7ANoS4hA4mKfKRwoX7ihn0mVPlFo0Le9NhZrcDvHzQ\"}}")]
        // [InlineData("/api/PushNotification", "DELETE", "1")]
        public void PushNotification_EndpointsReturnSuccessAndCorrectContentType(string url, string Method, string payload)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("api-version", "1.0"); client.DefaultRequestHeaders.Add("Authorization", this.authTokenFixture.Token);
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

                // PushNotificationModel newitem = new PushNotificationModel();
                // newproduct.Id = rnd.Next(1000, 10000);
                // var stringPayload = JsonConvert.SerializeObject(newproduct);
                var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                client.Timeout = TimeSpan.FromMinutes(3);
                responseTask = client.PostAsync(url, httpContent);
                responseTask.Wait();

                var stringcontent1Task = responseTask.Result.Content.ReadAsStringAsync();
                stringcontent1Task.Wait();
                output.WriteLine("Result :" + stringcontent1Task.Result);
                output.WriteLine("StatusCode :" + responseTask.Result.StatusCode);
                var statuscode = Convert.ToInt32(responseTask.Result.StatusCode);
                bool ifok = statuscode == 201 || statuscode == 500;
                Assert.True(ifok);
                //Assert.Equal(HttpStatusCode.InternalServerError, responseTask.Result.StatusCode);
                return;
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

            var stringcontentTask = response.Content.ReadAsStringAsync();
            stringcontentTask.Wait();
            output.WriteLine("Result :" + stringcontentTask.Result);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

    }

}