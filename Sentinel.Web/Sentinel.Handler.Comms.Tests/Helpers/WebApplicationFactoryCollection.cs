using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Handler.Comms.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.Handler.Comms.Startup>>, ICollectionFixture<AuthTokenFixture>, ICollectionFixture<CustomWebApplicationFactory>
    {


    }
}