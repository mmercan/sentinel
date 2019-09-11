using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Mercan.HealthChecks.Common.Checks
{
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.

    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddHttpGetCheck(this IHealthChecksBuilder builder, string url)
        {
            //return builder.AddCheck($"UrlGetCheck {url}", new HttpGetCheck(url));
            return builder.AddTypeActivatedCheck<HttpGetCheck>($"UrlGetCheck {url}", null, null, url);
        }
    }
    public class HttpGetCheck : IHealthCheck
    {
        private string url;
        private ILogger<HttpGetCheck> logger;

        public HttpGetCheck(ILogger<HttpGetCheck> logger, string url)
        {
            this.url = url;
            this.logger = logger;
            logger.LogCritical("HttpGetCheck Init");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDictionary<string, Object> data = new Dictionary<string, object>();
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    data.Add("content", content);
                    logger.LogCritical("HttpGetCheck Healthy");
                }
                catch (Exception ex)
                {
                    logger.LogCritical("HttpGetCheck Unhealthy");
                    return HealthCheckResult.Unhealthy(ex.Message, ex);
                }
            }
            ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
            return HealthCheckResult.Healthy($"{url} is Healthy", rodata);
        }
    }
}