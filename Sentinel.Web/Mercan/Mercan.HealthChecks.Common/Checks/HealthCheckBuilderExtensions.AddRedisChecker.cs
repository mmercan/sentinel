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
using EasyNetQ;
using StackExchange.Redis;

namespace Mercan.HealthChecks.Common.Checks
{
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.

    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            return builder.AddCheck($"AddRedisHealthCheck", new RabbitMQHealthCheck(connectionString));
        }
    }
    public class AddRedisHealthCheck : IHealthCheck
    {
        public static readonly string HealthCheckName = "AddRedisHealthCheck";
        string connectionString;
        public AddRedisHealthCheck(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDictionary<string, Object> data = new Dictionary<string, object>();
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
        }
    }
}