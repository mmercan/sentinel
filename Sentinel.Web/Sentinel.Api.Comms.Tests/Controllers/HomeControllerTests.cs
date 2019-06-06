using System;
using Xunit;
using Sentinel.Api.Comms;
using Sentinel.Api.Comms.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Sentinel.Api.Comms.Tests
{


    public class HomeControllerTests
    {
        private readonly ITestOutputHelper output;

        public HomeControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Home_Index_Returns_NoError()
        {
            output.WriteLine("The third Element is ");
            ILogger<HomeController> logger = null;
            IConfiguration configuration = null;

            var home = new HomeController(logger, configuration);
            //            home.Index().ToString().Should().Be("Blah");
            //  Assert.False(true);
          
        }
    }
}
