using System;
using System.Collections.Generic;
using System.Net.Http;
using Mercan.HealthChecks.Network.HttpRequest;
using Microsoft.Extensions.DependencyInjection;
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
            var service = new HttpRequestFactoryService(options);
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
            var service = new HttpRequestFactoryService(options);
            service.logger = new Logger<HttpRequestFactoryService>(factory);
            service.Get("/");
            // service.Get("https://google.com", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6InU0T2ZORl");
        }

        [Fact]
        public void RunHealthCheckwithContentConverter()
        {
            HttpRequest.Models.HttpClientOptions httpClientOptions = new HttpRequest.Models.HttpClientOptions
            {
                BaseAddress = "https://jsonplaceholder.typicode.com/todos/1",
                RequestContentType = "Json/application",
                DefaultRequestHeaders = new Dictionary<string, string>()
            };

            IOptions<HttpRequest.Models.HttpClientOptions> options = Microsoft.Extensions.Options.Options.Create(httpClientOptions);
            var service = new HttpRequestFactoryService(options);
            service.logger = new Logger<HttpRequestFactoryService>(factory);
            var result = service.Get("/");
            result.Item1.ContentAsType<MockData>();
            result.Item1.ContentAsJson();
            result.Item1.ContentAsString();

            Mercan.HealthChecks.Network.HttpResponseExtensions.ContentAsType<MockData>(result.Item1);
            Mercan.HealthChecks.Network.HttpResponseExtensions.ContentAsJson(result.Item1);
            Mercan.HealthChecks.Network.HttpResponseExtensions.ContentAsString(result.Item1);


            service.Get("https://google.com", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6InU0T2ZORl");
        }

        [Fact]
        public void RunHealthCheckforwrongUrls()
        {
            HttpRequest.Models.HttpClientOptions httpClientOptions = new HttpRequest.Models.HttpClientOptions
            {
                BaseAddress = "https://ZKFDSJhlakfdsjahfld.com",
                RequestContentType = "Json/application",
                DefaultRequestHeaders = new Dictionary<string, string>()
            };

            IOptions<HttpRequest.Models.HttpClientOptions> options = Microsoft.Extensions.Options.Options.Create(httpClientOptions);
            var service = new HttpRequestFactoryService(options);
            service.logger = new Logger<HttpRequestFactoryService>(factory);
            var result = service.Get("/");
        }
        [Fact]
        public void RunHealthCheckfor404Urls()
        {
            HttpRequest.Models.HttpClientOptions httpClientOptions = new HttpRequest.Models.HttpClientOptions
            {
                BaseAddress = "https://www.google.com/blah",
                RequestContentType = "Json/application",
                DefaultRequestHeaders = new Dictionary<string, string>()
            };

            IOptions<HttpRequest.Models.HttpClientOptions> options = Microsoft.Extensions.Options.Options.Create(httpClientOptions);
            var service = new HttpRequestFactoryService(options);
            service.logger = new Logger<HttpRequestFactoryService>(factory);
            var result = service.Get("/");

        }


        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddNetworkHealthCheck("https://google.com");
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }
    }

    public class MockData
    {
        public string userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }

    }
}