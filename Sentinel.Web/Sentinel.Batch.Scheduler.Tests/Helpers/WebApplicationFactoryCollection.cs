using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Batch.Scheduler.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.Batch.Scheduler.Startup>>, ICollectionFixture<AuthTokenFixture>
    {


    }
}