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

namespace Mercan.HealthChecks.Common.Checks
{
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.

    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddDIHealthCheck(this IHealthChecksBuilder builder, IServiceCollection services)
        {
            //return builder.AddCheck($"DIHealthCheck", new DIHealthCheck(services));
            return builder.AddTypeActivatedCheck<DIHealthCheck>($"DIHealthCheck", null, null, services);
        }
    }
    public class DIHealthCheck : IHealthCheck
    {
        private IServiceCollection services;
        private ILogger<DIHealthCheck> logger;

        public DIHealthCheck(ILogger<DIHealthCheck> logger, IServiceCollection services)
        {
            this.services = services;
            this.logger = logger;
            logger.LogCritical("DIHealthCheck Init");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {

                bool failedAny = false;
                int order = 0;
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "DIHealthCheck");

                List<Type> exceptions = new List<Type>{
                typeof(Microsoft.Extensions.Options.IOptions<>),
                typeof(Microsoft.Extensions.Options.OptionsCache<>),
                typeof(Microsoft.Extensions.Options.IOptionsSnapshot<>),
                typeof(Microsoft.Extensions.Options.IOptionsMonitor<>),
                typeof(Microsoft.Extensions.Options.IOptionsFactory<>),
                // typeof(Microsoft.Extensions.Logging.Configuration.ILoggerProviderConfiguration<>),
                };
                try
                {
                    var serviceprovider = services.BuildServiceProvider();
                    foreach (var service in services)
                    {
                        order++;
                        try
                        {
                            bool skip = false;
                            if (exceptions.Contains(service.ServiceType))
                            {
                                skip = true;
                                data.Add(order.ToString() + " " + service.ServiceType.FullName, "Skipped");
                            }
                            if (!skip)
                            {
                                if (service.ServiceType.ContainsGenericParameters)
                                {
                                    var t = service.ServiceType.MakeGenericType(typeof(DIHealthCheck));
                                    var instance = serviceprovider.GetService(t);
                                    data.Add(order.ToString() + " " + instance.GetType().FullName, "Success");
                                }
                                else
                                {
                                    var instance = serviceprovider.GetService(service.ServiceType);
                                    data.Add(order.ToString() + " " + instance.GetType().FullName, "Success");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Add(order.ToString() + " " + service.ServiceType.FullName, "Failed Exception " + ex.Message);
                            failedAny = true;
                        }
                    }
                    if (failedAny)
                    {
                        string description = "Dependency Injection is failed for some services";
                        ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                        return HealthCheckResult.Degraded(description, data: rodata);
                    }
                    else
                    {
                        string description = "DependencyInjection is healthy for all services";
                        ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                        return HealthCheckResult.Healthy(description, rodata);              
                    }
                }
                catch (Exception ex)
                {
                    string description = "DependencyInjection is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }
            });
        }
    }
}