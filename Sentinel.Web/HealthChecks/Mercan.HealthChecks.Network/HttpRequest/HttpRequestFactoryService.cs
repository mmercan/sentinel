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
        public ILogger<HttpRequestFactoryService> logger { get; set; }
        public HttpRequestFactoryService(IOptions<HttpClientOptions> httpClientOptions)
        {
            this.httpClientOptions = httpClientOptions.Value;
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
            return Get(builder, urlSuffix);
        }

        public Tuple<HttpResponseMessage, string> Get(HttpRequestBuilder builder, string urlSuffix)
        {
            string url = httpClientOptions.BaseAddress + urlSuffix;
            Task<HttpResponseMessage> taskresult = null;
            try
            {
                taskresult = builder.SendAsync();
                taskresult.Wait();
                return Tuple.Create(taskresult.Result, url);
            }
            catch (AggregateException ea)
            {
                return HandleAggregateException(ea, url);
            }
        }

        public Tuple<HttpResponseMessage, string> HandleAggregateException(AggregateException ea, string url)
        {
            foreach (var ex in ea.Flatten().InnerExceptions)
            {
                HttpResponseMessage respose = null;
                string errormessage = ex?.InnerException?.Message;
                Exception sendex = ex?.InnerException;
                if (errormessage == null && ex?.Message != null)
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


                if (ex is HttpRequestException && ex.InnerException != null && ex.InnerException.Message == "The server name or address could not be resolved")
                {
                    respose = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                }
                else if (ex is HttpRequestException)
                {
                    respose = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
                else
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
