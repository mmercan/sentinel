using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Mercan.HealthChecks.Network
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddNetworkHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            return builder.AddTypeActivatedCheck<NetworkHealthCheck>($"NetworkHealthCheck {connectionString}", null, null, connectionString);
        }
    }
    public class NetworkHealthCheck : IHealthCheck
    {
        private readonly string connectionString;
        public NetworkHealthCheck(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "NetworkHealthCheck");
                try
                {
                    string description = "NetworkHealthCheck is healthy";
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (Exception ex)
                {
                    string description = "NetworkHealthCheck is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }
            });
        }
    }
}