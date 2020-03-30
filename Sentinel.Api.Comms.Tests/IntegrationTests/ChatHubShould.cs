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

using Microsoft.AspNetCore.SignalR;
using Moq;
using Sentinel.Api.Comms.Hubs;
using System.Threading;

namespace Sentinel.Api.Comms.Tests.IntegrationTests
{
    [Collection("WebApplicationFactory")]
    public class ChatHubShould
    {


        private WebApplicationFactory<Startup> factory;
        AuthTokenFixture authTokenFixture;
        private ITestOutputHelper output;

        public ChatHubShould(CustomWebApplicationFactory factory, AuthTokenFixture authTokenFixture, ITestOutputHelper output)
        {
            this.factory = factory;

            this.output = output;
            this.authTokenFixture = authTokenFixture;
        }


        [Fact]
        public async void Connect()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<HubCallerContext> mockContext = new Mock<HubCallerContext>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);


            ChatHub simpleHub = new ChatHub()
            {
                Clients = mockClients.Object,
                Context = mockContext.Object
            };

            // act
            await simpleHub.Send("welcome");

            // await simpleHub.OnConnectedAsync();
            // await simpleHub.OnDisconnectedAsync(null);

            // assert
            mockClients.Verify(clients => clients.All, Times.Once);

            // mockClientProxy.Verify(
            //     clientProxy => clientProxy.SendCoreAsync(
            //         "Send",
            //         It.Is<object[]>(o => o != null && o.Length == 1 && ((object[])o[0]).Length == 3),
            //         default(CancellationToken)),
            //     Times.Once);

            // mockClientProxy.Verify(
            //     clientProxy => clientProxy.SendCoreAsync("Send",
            //         It.Is<object[]>(o => o != null && o.Length == 1 && ((string)o[0]).Length == 3),
            //     default(CancellationToken)),
            // Times.Once);
        }
    }
}