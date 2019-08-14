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
        string base64 = "MIICYzCCAcygAwIBAgIBADANBgkqhkiG9w0BAQUFADAuMQswCQYDVQQGEwJVUzEM  MAoGA1UEChMDSUJNMREwDwYDVQQLEwhMb2NhbCBDQTAeFw05OTEyMjIwNTAwMDBa  Fw0wMDEyMjMwNDU5NTlaMC4xCzAJBgNVBAYTAlVTMQwwCgYDVQQKEwNJQk0xETAP  BgNVBAsTCExvY2FsIENBMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD2bZEo  7xGaX2/0GHkrNFZvlxBou9v1Jmt/PDiTMPve8r9FeJAQ0QdvFST/0JPQYD20rH0b  imdDLgNdNynmyRoS2S/IInfpmf69iyc2G0TPyRvmHIiOZbdCd+YBHQi1adkj17ND  cWj6S14tVurFX73zx0sNoMS79q3tuXKrDsxeuwIDAQABo4GQMIGNMEsGCVUdDwGG  +EIBDQQ+EzxHZW5lcmF0ZWQgYnkgdGhlIFNlY3VyZVdheSBTZWN1cml0eSBTZXJ2  ZXIgZm9yIE9TLzM5MCAoUkFDRikwDgYDVR0PAQH/BAQDAgAGMA8GA1UdEwEB/wQF  MAMBAf8wHQYDVR0OBBYEFJ3+ocRyCTJw067dLSwr/nalx6YMMA0GCSqGSIb3DQEB  BQUAA4GBAMaQzt+zaj1GU77yzlr8iiMBXgdQrwsZZWJo5exnAucJAEYQZmOfyLiM  D6oYq+ZnfvM0n8G/Y79q8nhwvuxpYOnRSAXFp6xSkrIOeZtJMY1h00LKp/JX3Ng1  svZ2agE126JHsQ0bhzN5TKsYfbwfTwfjdWAGy6Vf1nYi/rO+ryMO";
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
        public void RunHttpRequestFactoryServiceWithBuilder()
        {
            HttpRequest.Models.HttpClientOptions httpClientOptions = new HttpRequest.Models.HttpClientOptions
            {
                BaseAddress = "https://www.google.com/blah",
                RequestContentType = "Json/application",
                DefaultRequestHeaders = new Dictionary<string, string>(){
                    {"cache-control","max-age=0"}
                }
            };

            IOptions<HttpRequest.Models.HttpClientOptions> options = Microsoft.Extensions.Options.Options.Create(httpClientOptions);
            var service = new HttpRequestFactoryService(options);
            service.logger = new Logger<HttpRequestFactoryService>(factory);

            var builder = new HttpRequestBuilder()
                    .AddMethod(HttpMethod.Get)
                    .AddRequestUri("https://www.google.com/blah")
                    .AddRequestContentType(httpClientOptions.RequestContentType)
                    .AddClientCertificate(httpClientOptions.ClientCertificateBase64)
                    .AddHeaders(httpClientOptions.DefaultRequestHeaders)
                    .AddAcceptHeader("Json/application")
                    .AddTimeout(TimeSpan.FromMinutes(3))
                    .AddClientCertificate(base64)
                    .AddSubscriptionKey("123456789")
                    .AddLogger(service.logger);


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