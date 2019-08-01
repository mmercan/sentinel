using System.Collections.Generic;
using Mercan.HealthChecks.Network.HttpRequest;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace Mercan.HealthChecks.Network.Tests.Checks
{
    public class HttpRequestFactoryServiceShould
    {

        public HttpRequestFactoryServiceShould()
        {

        }
        ILoggerFactory factory = new LoggerFactory();
        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateaInstance()
        {
            HttpRequest.Models.HttpClientOptions httpClientOptions = new HttpRequest.Models.HttpClientOptions
            {
                BaseAddress = "https://google.com",
                RequestContentType = "Json/application",
                DefaultRequestHeaders = new Dictionary<string, string>()
            };

            IOptions<HttpRequest.Models.HttpClientOptions> options = Microsoft.Extensions.Options.Options.Create(httpClientOptions);

            HttpRequestBuilder builder = new HttpRequestBuilder();

            var service = new HttpRequestFactoryService(options, builder);
        }

        [Fact]
        public void RunHealthCheck()
        {
            HttpRequest.Models.HttpClientOptions httpClientOptions = new HttpRequest.Models.HttpClientOptions
            {
                BaseAddress = "https://google.com",
                RequestContentType = "Json/application",
                DefaultRequestHeaders = new Dictionary<string, string>()

            };

            IOptions<HttpRequest.Models.HttpClientOptions> options = Microsoft.Extensions.Options.Options.Create(httpClientOptions);

            HttpRequestBuilder builder = new HttpRequestBuilder();

            var service = new HttpRequestFactoryService(options, builder);
            service.logger = new Logger<HttpRequestFactoryService>(factory);
            service.Get("/");
            // service.Get("https://google.com", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6InU0T2ZORl");
        }
    }
}