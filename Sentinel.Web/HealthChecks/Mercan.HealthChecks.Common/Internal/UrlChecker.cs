using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Common
{
    public class UrlChecker
    {
        private readonly Func<HttpResponseMessage, ValueTask<HealthCheckResult>> _checkFunc;
        private readonly string _url;

        public UrlChecker(Func<HttpResponseMessage, ValueTask<HealthCheckResult>> checkFunc, string url)
        {
            Guard.ArgumentNotNull(nameof(checkFunc), checkFunc);
            Guard.ArgumentNotNullOrEmpty(nameof(url), url);

            _checkFunc = checkFunc;
            _url = url;
        }

        public HealthStatus PartiallyHealthyStatus { get; set; } = HealthStatus.Degraded;

        public async Task<HealthCheckResult> CheckAsync()
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(_url).ConfigureAwait(false);
                    return await _checkFunc(response);
                }
                catch (Exception ex)
                {
                    Dictionary<string, object> data = new Dictionary<string, object> { { "url", _url } };
                    data.Add("type", "UrlChecker");
                    return HealthCheckResult.Unhealthy($"Exception during check: {ex.GetType().FullName}", null, data);
                }
            }
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            return httpClient;
        }

        public static async ValueTask<HealthCheckResult> DefaultUrlCheck(HttpResponseMessage response)
        {
            HealthStatus status = response.IsSuccessStatusCode ? HealthStatus.Healthy : HealthStatus.Unhealthy;
            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "url", response.RequestMessage.RequestUri.ToString()},
                    { "status", (int) response.StatusCode },
                    { "reason", response.ReasonPhrase },
                    { "body", await response.Content?.ReadAsStringAsync() },
                    { "type", "UrlChecker"}
                };
            return new HealthCheckResult(status, $"status code {response.StatusCode} ({(int)response.StatusCode})", null, data);
        }

        public static async ValueTask<HealthCheckResult> ComposeHealthCheckStatus(HttpResponseMessage response)
        {
            HealthStatus status = response.IsSuccessStatusCode ? HealthStatus.Healthy : HealthStatus.Unhealthy;
            string responseContent = await response.Content?.ReadAsStringAsync();
            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "url", response.RequestMessage.RequestUri.ToString()},
                    { "status", (int) response.StatusCode },
                    { "reason", response.ReasonPhrase },
                    { "body", responseContent },
                    { "type", "UrlChecker"}
                };
            return new HealthCheckResult(status, $"status code {response.StatusCode} ({(int)response.StatusCode})", null, data);
        }

        protected virtual HttpClient GetHttpClient()
        {
            return new HttpClient();
        }
    }
}
