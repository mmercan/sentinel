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
using System.Diagnostics;

namespace Mercan.HealthChecks.Common.Checks
{

    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddProcessList(this IHealthChecksBuilder builder, TimeSpan? cacheDuration = null)
        {
            return builder.AddCheck($"AddProcessList", new ProcessListHealthChecks());
        }
    }
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.
    public class ProcessListHealthChecks : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDictionary<string, Object> data = new Dictionary<string, object>();
            UInt32 totalsize = 0;
            int number = 0;
            foreach (var aProc in Process.GetProcesses())
            {
                number++;
                data.Add(aProc.ProcessName + " " + number.ToString(), aProc.WorkingSet64 / 1024);
                totalsize += Convert.ToUInt32(aProc.WorkingSet64 / 1024.0);
            }

            data.Add("Total Memory", totalsize);
            ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
            string description = "ProcessList Total Memory Usage " + totalsize + " MB";
            return HealthCheckResult.Healthy(description, rodata);

        }
    }
}