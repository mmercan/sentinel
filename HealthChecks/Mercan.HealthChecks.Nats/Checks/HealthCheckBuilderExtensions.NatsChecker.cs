using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Mercan.HealthChecks.Nats
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddNatsHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            return builder.AddTypeActivatedCheck<NatsHealthCheck>($"NatsHealthCheck {connectionString}", null, null, connectionString);
        }
    }
    public class NatsHealthCheck : IHealthCheck
    {
       private readonly string connectionString;
        public NatsHealthCheck(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "NatsHealthCheck");
                try
                {
                    string description = "NatsHealthCheck is healthy";
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (Exception ex)
                {
                    string description = "NatsHealthCheck is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }
            });
        }
    }
}