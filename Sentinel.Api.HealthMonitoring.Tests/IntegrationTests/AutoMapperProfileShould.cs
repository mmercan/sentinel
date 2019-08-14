using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Sentinel.Api.HealthMonitoring;
using Sentinel.Api.HealthMonitoring.Tests.Helpers;

namespace Sentinel.Api.HealthMonitoring.Tests.IntegrationTests
{
    [Collection("WebApplicationFactory")]
    public class AutoMapperProfileShould
    {

        private CustomWebApplicationFactory factory;
        private ITestOutputHelper output;

        public AutoMapperProfileShould(CustomWebApplicationFactory factory, ITestOutputHelper output)
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
