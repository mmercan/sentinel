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



namespace Mercan.HealthChecks.Common.Checks
{
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.
    public class AddMinValueCheck<T> : IHealthCheck where T : class, IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDictionary<string, Object> data = new Dictionary<string, object>();
            string description = "AddMinValueCheck is healthy";
            ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
            return HealthCheckResult.Healthy(description, rodata);

        }
    }
}