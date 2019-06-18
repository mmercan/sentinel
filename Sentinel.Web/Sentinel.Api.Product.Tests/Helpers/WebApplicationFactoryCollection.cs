using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Api.Product.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.Api.Product.Startup>>, ICollectionFixture<AuthTokenFixture>
    {


    }
}