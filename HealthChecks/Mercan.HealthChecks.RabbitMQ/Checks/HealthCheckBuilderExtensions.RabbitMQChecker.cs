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

namespace Mercan.HealthChecks.RabbitMQ
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddRabbitMQHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            return builder.AddTypeActivatedCheck<RabbitMQHealthCheck>($"RabbitMQHealthCheck : {connectionString}", null, null, connectionString);
        }

        public static IHealthChecksBuilder AddRabbitMQHealthCheckWithDiIBus(this IHealthChecksBuilder builder)
        {
            return builder.AddTypeActivatedCheck<RabbitMQHealthCheckFromBus>($"RabbitMQHealthCheck_DI_IBUS");
        }
    }
    public class RabbitMQHealthCheck : IHealthCheck
    {
        private readonly string connectionString;
        public RabbitMQHealthCheck(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                using (var bus = RabbitHutch.CreateBus(connectionString))
                {
                    var rabq = new RabbitMQHealthCheckFromBus(bus);
                    var resultask = rabq.CheckHealthAsync(context, cancellationToken);
                    resultask.Wait();
                    return resultask.Result;
                }
            });
        }
    }


    public class RabbitMQHealthCheckFromBus : IHealthCheck
    {
        private readonly EasyNetQ.IBus bus;
        public RabbitMQHealthCheckFromBus(EasyNetQ.IBus bus)
        {
            this.bus = bus;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "RabbitMQHealthCheckFromBus");
                try
                {
                    bus.Publish("Test", "healthcheck.rabbitmq");
                    data.Add("Connected", bus.IsConnected);
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