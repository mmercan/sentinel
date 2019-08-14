using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Sentinel.Batch.Scheduler;

namespace Sentinel.Batch.Scheduler.Tests.IntegrationTests
{
    [Collection("WebApplicationFactory")]
    public class AutoMapperProfileShould
    {

        private WebApplicationFactory<Startup> factory;
        private ITestOutputHelper output;

        public AutoMapperProfileShould(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }

        [Fact]
        public void CreateAnInstance()
        {
            AutoMapperProfile profile = new AutoMapperProfile();
        }
    }
}
