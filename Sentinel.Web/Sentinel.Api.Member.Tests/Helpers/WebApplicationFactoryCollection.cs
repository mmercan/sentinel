using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Api.Member.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.Api.Member.Startup>>, ICollectionFixture<AuthTokenFixture>
    {


    }
}