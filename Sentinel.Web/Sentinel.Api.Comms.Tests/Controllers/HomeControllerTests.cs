using System;
using Xunit;
using Sentinel.Api.Comms;
using Sentinel.Api.Comms.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using FluentAssertions;

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
            home.Index().Should().NotBeNull();//.ToString().Should().Be("Blah");
            //  Assert.False(true);

        }


        [Fact]
        public void Home_About_Returns_NoError()
        {
            output.WriteLine("The third Element is ");
            ILogger<HomeController> logger = null;
            IConfiguration configuration = null;

            var home = new HomeController(logger, configuration);
            home.About().Should().NotBeNull();
        }

        public void Home_Contact_Returns_NoError()
        {
            output.WriteLine("The third Element is ");
            ILogger<HomeController> logger = null;
            IConfiguration configuration = null;

            var home = new HomeController(logger, configuration);
            home.Contact().Should().NotBeNull();
        }


        public void Home_Privacy_Returns_NoError()
        {
            output.WriteLine("The third Element is ");
            ILogger<HomeController> logger = null;
            IConfiguration configuration = null;

            var home = new HomeController(logger, configuration);
            home.Privacy().Should().NotBeNull();
        }


        public void Home_Error_Returns_NoError()
        {
            output.WriteLine("The third Element is ");
            ILogger<HomeController> logger = null;
            IConfiguration configuration = null;

            var home = new HomeController(logger, configuration);
            home.Error().Should().NotBeNull();
        }
    }
}