using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Api.Scheduler.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.Api.Scheduler.Startup>>, ICollectionFixture<AuthTokenFixture>
    {


    }
}