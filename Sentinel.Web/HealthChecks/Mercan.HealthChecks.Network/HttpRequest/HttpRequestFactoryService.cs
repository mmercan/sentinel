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
        public HttpClientOptions httpClientOptions;
        private HttpRequestBuilder httpRequestBuilder;

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
                    //.AddBearerToken(bearerToken)
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
                foreach (var ex in ea.Flatten().InnerExceptions)
                {
                    if (logger != null)
                    {
                        if (ex.InnerException != null && ex.InnerException.Message != null)
                        {
                            logger.LogError(ex.InnerException.Message + " Url : " + url, ex.InnerException);
                        }
                        else
                        {
                            logger.LogError(ex.Message + " Url : " + url, ex);
                        }
                    }
                    if (ex is HttpRequestException)
                    {
                        if (ex.InnerException != null && ex.InnerException.Message == "The server name or address could not be resolved")
                        {
                            HttpResponseMessage respose = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                            respose.Content = new StringContent(ex.InnerException.Message + " url : " + url);
                            return Tuple.Create(respose, url);
                        }
                    }
                    throw ex;
                }
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
                foreach (var ex in ea.Flatten().InnerExceptions)
                {
                    if (logger != null)
                    {

                        if (ex.InnerException != null && ex.InnerException.Message != null)
                        {
                            logger.LogError(ex.InnerException.Message + " Url : " + url, ex.InnerException);
                        }
                        else
                        {
                            logger.LogError(ex.Message + " Url : " + url, ex);
                        }
                    }
                    if (ex is HttpRequestException)
                    {
                        if (ex.InnerException != null && ex.InnerException.Message == "The server name or address could not be resolved")
                        {
                            HttpResponseMessage respose = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                            respose.Content = new StringContent(ex.InnerException.Message + " url : " + url);
                            return Tuple.Create(respose, url);
                        }
                    }
                    throw ex;
                }
            }
            return Tuple.Create(taskresult.Result, url);
        }
    }
}
