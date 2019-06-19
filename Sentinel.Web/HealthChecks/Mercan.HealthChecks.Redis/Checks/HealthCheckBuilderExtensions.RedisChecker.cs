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
using StackExchange.Redis;

namespace Mercan.HealthChecks.Redis
{
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.

    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            return builder.AddTypeActivatedCheck<RedisHealthCheck>($"RedisHealthCheck {connectionString}", null, null, connectionString);
        }
    }
    public class RedisHealthCheck : IHealthCheck
    {
        public static readonly string HealthCheckName = "RedisHealthCheck";
        string connectionString;
        public RedisHealthCheck(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "RedisHealthCheck");
                try
                {
                    var redisconnection = ConnectionMultiplexer.Connect(connectionString);
                    var database = redisconnection.GetDatabase();
                    data.Add("Connected", redisconnection.IsConnected);
                    string description = "RedisHealthCheck is healthy";
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (Exception ex)
                {
                    string description = "RedisHealthCheck is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }

            });
        }
    }
}