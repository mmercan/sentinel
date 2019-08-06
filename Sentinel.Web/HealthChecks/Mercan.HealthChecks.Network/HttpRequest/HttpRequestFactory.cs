using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.HttpRequest
{
    public static class HttpRequestFactory
    {
        public static async Task<HttpResponseMessage> GetAsync(string requestUri) => await GetAsync(requestUri, "", "", "", "");
        public static async Task<HttpResponseMessage> GetWithClientCertAsync(string requestUri, string base64Certificate, string certificatePassword) => await GetAsync(requestUri, "", base64Certificate, certificatePassword, "");


        public static async Task<HttpResponseMessage> GetAsync(string requestUri, string bearerToken, string base64Certificate, string certificatePassword, string subscriptionKey)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken)
                                .AddClientCertificate(base64Certificate, certificatePassword)
                                .AddSubscriptionKey(subscriptionKey)
                                .AddTimeout(TimeSpan.FromMinutes(3));
            return await builder.SendAsync();
        }


        public static HttpResponseMessage Get(string requestUri, string bearerToken, string base64Certificate, string certificatePassword, string subscriptionKey)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken)
                                .AddClientCertificate(base64Certificate, certificatePassword)
                                .AddSubscriptionKey(subscriptionKey);

            var taskresult = builder.SendAsync();
            taskresult.Wait();
            return taskresult.Result;
        }

        public static async Task<HttpResponseMessage> Post(string requestUri, object value) => await Post(requestUri, value, "");
        public static async Task<HttpResponseMessage> Post(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(string requestUri, object value) => await Put(requestUri, value, "");
        public static async Task<HttpResponseMessage> Put(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Patch(string requestUri, object value) => await Patch(requestUri, value, "");
        public static async Task<HttpResponseMessage> Patch(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(new HttpMethod("PATCH"))
                                .AddRequestUri(requestUri)
                                .AddContent(new PatchContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(string requestUri) => await Delete(requestUri, "");
        public static async Task<HttpResponseMessage> Delete(string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> PostFile(string requestUri, string filePath, string apiParamName) => await PostFile(requestUri, filePath, apiParamName, "");
        public static async Task<HttpResponseMessage> PostFile(string requestUri, string filePath, string apiParamName, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new FileContent(filePath, apiParamName))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }
    }
}
