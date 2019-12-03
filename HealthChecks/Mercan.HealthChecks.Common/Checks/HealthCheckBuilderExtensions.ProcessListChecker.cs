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
            return builder.AddTypeActivatedCheck<ProcessListHealthChecks>($"ProcessList (KB)");
        }
    }
    public class ProcessListHealthChecks : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "ProcessListHealthChecks");
                UInt32 totalsize = 0;
                Double TotalProcessorTime = 0;
                int number = 0;
                foreach (var aProc in Process.GetProcesses())
                {
                    number++;
                    data.Add(aProc.ProcessName + "_Memory " + number.ToString(), aProc.WorkingSet64 / 1024);
                    try
                    {
                        data.Add(aProc.ProcessName + "_TotalProcessorTime " + number.ToString(), aProc.TotalProcessorTime.TotalSeconds);
                        TotalProcessorTime += aProc.TotalProcessorTime.TotalSeconds;
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrWhiteSpace(ex.Message))
                        {
                            data.Add("error", ex.Message);
                        }
                        // data.Add(aProc.ProcessName + "_TotalProcessorTime " + number.ToString(), "Failed to load");
                    }
                    totalsize += Convert.ToUInt32(aProc.WorkingSet64 / 1024.0);
                }

                data.Add("Total Memory", totalsize);
                data.Add("Total TotalProcessorTime", TotalProcessorTime);
                ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                string description = "ProcessList Total Memory Usage " + totalsize + " MB";
                return HealthCheckResult.Healthy(description, rodata);
            });
        }
    }
}