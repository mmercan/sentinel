using Mercan.HealthChecks.Network.HttpRequest.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.HttpRequest
{
    public class HttpRequestFactoryService
    {
        public HttpClientOptions httpClientOptions { get; set; }
        private readonly HttpRequestBuilder httpRequestBuilder;

        public ILogger<HttpRequestFactoryService> logger { get; set; }
        public HttpRequestFactoryService(IOptions<HttpClientOptions> httpClientOptions, HttpRequestBuilder httpRequestBuilder)
        {
            this.httpClientOptions = httpClientOptions.Value;
            this.httpRequestBuilder = httpRequestBuilder;
        }

        public Tuple<HttpResponseMessage, string> Get(string urlSuffix)
        {
            string url = httpClientOptions.BaseAddress + urlSuffix;
            var builder = new HttpRequestBuilder()
                    .AddMethod(HttpMethod.Get)
                    .AddRequestUri(url)
                    .AddRequestContentType(httpClientOptions.RequestContentType)
                    .AddClientCertificate(httpClientOptions.ClientCertificateBase64)
                    .AddHeaders(httpClientOptions.DefaultRequestHeaders)
                    .AddLogger(logger);

            Task<HttpResponseMessage> taskresult = null;
            try
            {
                taskresult = builder.SendAsync();
                taskresult.Wait();
            }
            catch (AggregateException ea)
            {
                HandleAggregateException(ea, url);
            }
            return Tuple.Create(taskresult.Result, url);
        }


        public Tuple<HttpResponseMessage, string> Get(string urlSuffix, string jwt)
        {
            string url = httpClientOptions.BaseAddress + urlSuffix;
            var builder = new HttpRequestBuilder()
                    .AddMethod(HttpMethod.Get)
                    .AddRequestUri(url)
                    .AddBearerToken(jwt)
                    .AddRequestContentType(httpClientOptions.RequestContentType)
                    .AddClientCertificate(httpClientOptions.ClientCertificateBase64)
                    .AddHeaders(httpClientOptions.DefaultRequestHeaders)
                    .AddLogger(logger);

            Task<HttpResponseMessage> taskresult = null;
            try
            {
                taskresult = builder.SendAsync();
                taskresult.Wait();
            }
            catch (AggregateException ea)
            {
                return HandleAggregateException(ea, url);
            }
            return Tuple.Create(taskresult.Result, url);
        }


        public Tuple<HttpResponseMessage, string> HandleAggregateException(AggregateException ea, string url)
        {
            foreach (var ex in ea.Flatten().InnerExceptions)
            {
                HttpResponseMessage respose = null;
                string errormessage = ex?.InnerException?.Message;
                Exception sendex = ex?.InnerException;
                if (errormessage == null && ex.Message != null)
                {
                    errormessage = ex.Message;
                    sendex = ex;
                }
                else
                {
                    errormessage = "InnerException.Message is Null";
                }

                if (logger != null)
                {
                    logger.LogError(errormessage + " Url : " + url, sendex);
                }
                if (ex is HttpRequestException)
                {
                    if (ex.InnerException != null && ex.InnerException.Message == "The server name or address could not be resolved")
                    {
                        respose = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                    }
                    else
                    {
                        respose = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                    }
                }
                if (respose == null)
                {
                    respose = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
                }
                respose.Content = new StringContent(errormessage + " url : " + url);
                return Tuple.Create(respose, url);
            }
            HttpResponseMessage respose1 = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest); ;
            return Tuple.Create(respose1, url);
        }
    }
}
