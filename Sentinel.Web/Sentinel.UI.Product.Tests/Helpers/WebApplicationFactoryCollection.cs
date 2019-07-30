using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.UI.Product.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.UI.Product.Startup>>, ICollectionFixture<AuthTokenFixture>
    {


    }
}