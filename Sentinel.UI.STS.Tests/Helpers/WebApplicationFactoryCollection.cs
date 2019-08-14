using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.UI.STS.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.UI.STS.Startup>>, ICollectionFixture<AuthTokenFixture>
    {


    }
}