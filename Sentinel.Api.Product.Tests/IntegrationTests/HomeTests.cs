// using System;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Text;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Xunit;
// using Xunit.Abstractions;
// using Sentinel.Api.Product;
// using Sentinel.Api.Product.Tests.Helpers;
// using Sentinel.Model.Product.Dto;
// using Newtonsoft.Json;

// namespace Sentinel.Api.Product.Tests.IntegrationTests
// {

//     [Collection("WebApplicationFactory")]
//     public class HomeTests
//     {
//         private WebApplicationFactory<Startup> factory;
//         AuthTokenFixture authTokenFixture;
//         private ITestOutputHelper output;

//         public HomeTests(WebApplicationFactory<Startup> factory,AuthTokenFixture authTokenFixture ,ITestOutputHelper output)
//         {
//             this.factory = factory;
//             this.output = output;
//             this.authTokenFixture=authTokenFixture;
//          //  output.WriteLine("Token Received "+  this.authTokenFixture.Token);
//         }


//         [Theory]
//         //[InlineData("/")]
//         [InlineData("/Home/Index")]
//         [InlineData("/Home/About")]
//         [InlineData("/Home/Contact")]
//         [InlineData("/Home/Privacy")]
//         [InlineData("/Home/Error")]
//         public void Get_EndpointsReturnSuccessAndCorrectContentType(string url)
//         {
//             // Arrange
//             var client = factory.CreateClient();
//             //factory.WithWebHostBuilder()
//             // Act
//             var responseTask = client.GetAsync(url);
//             responseTask.Wait();
//             var response = responseTask.Result;
//             // Assert
//             response.EnsureSuccessStatusCode(); // Status Code 200-299
//             Assert.Equal("text/html; charset=utf-8",
//                 response.Content.Headers.ContentType.ToString());
//         }
//     }
// }
