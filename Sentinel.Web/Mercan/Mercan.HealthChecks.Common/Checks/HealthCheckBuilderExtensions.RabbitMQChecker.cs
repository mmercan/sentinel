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

namespace Mercan.HealthChecks.Common.Checks
{
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.

    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddRabbitMQHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            // return builder.AddCheck($"RabbitMQHealthCheck", new RabbitMQHealthCheck(connectionString));
            return builder.AddTypeActivatedCheck<RabbitMQHealthCheck>($"RabbitMQHealthCheck : {connectionString}", null, null, connectionString);
        }
    }
    public class RabbitMQHealthCheck : IHealthCheck
    {
        public static readonly string HealthCheckName = "RabbitMQHealthCheck";
        string connectionString;

        public RabbitMQHealthCheck(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                try
                {
                    using (var bus = RabbitHutch.CreateBus(connectionString))
                    {
                        bus.Publish("Test", "healthcheck.rabbitmq");
                        var connected = bus.IsConnected;
                        data.Add("Connected", bus.IsConnected);
                    }
                    string description = "RabbitMQHealthCheck is healthy";
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (Exception ex)
                {
                    string description = "RabbitMQHealthCheck is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }
            });
        }
    }
}