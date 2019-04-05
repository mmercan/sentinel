using System;
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
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.
    public class SystemInfoHealthChecks : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDictionary<string, Object> data = new Dictionary<string, object>();
          //  data.Add("TotalMemoryMB", RunQueryMB("SELECT MaxCapacity FROM Win32_PhysicalMemoryArray", "MaxCapacity"));
          //  data.Add("Win32_PerfRawData_PerfOS_Memory", RunQueryMB("SELECT * FROM Win32_PerfRawData_PerfOS_Memory"));

            data.Add("PrivateMemorySize64", Process.GetCurrentProcess().PrivateMemorySize64 / 1024);
            data.Add("VirtualMemorySize64", Process.GetCurrentProcess().VirtualMemorySize64 / 1024);
            data.Add("WorkingSet64", Process.GetCurrentProcess().WorkingSet64 / 1024);
            ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
            string description = "SystemInfoCheck";
            return HealthCheckResult.Healthy(description, rodata);

        }
    }
}