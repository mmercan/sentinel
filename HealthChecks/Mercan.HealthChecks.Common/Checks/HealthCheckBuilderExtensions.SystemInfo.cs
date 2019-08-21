using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Mercan.HealthChecks.Common.Checks
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddSystemInfoCheck(this IHealthChecksBuilder builder)
        {
            return builder.AddTypeActivatedCheck<SystemInfoHealthChecks>($"SystemInfo");
        }


        public static IHealthChecksBuilder AddPerformanceCounter(this IHealthChecksBuilder builder, String WMIClassName)
        {
            builder.AddCheck($"PerformanceCounter for " + WMIClassName, () =>
            {
                try
                {
                    IDictionary<string, Object> data = RunQueryforData("SELECT * FROM " + WMIClassName); //Win32_PerfRawData_PerfOS_Memory
                    data.Add("type", "PerformanceCounter");
                    data.Add("wmiClassName", WMIClassName);
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    string description = "PerformanceCounter for " + WMIClassName;
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (PlatformNotSupportedException ex)
                {
                    return HealthCheckResult.Degraded(ex.Message, ex, new Dictionary<string, object> { { "type", "PerformanceCounter" }, { "wmiClassName", WMIClassName } });
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy(ex.Message, ex, new Dictionary<string, object> { { "type", "PerformanceCounter" }, { "wmiClassName", WMIClassName } });
                }
            });
            return builder;
        }

        public static IHealthChecksBuilder AddPerformanceCounter(this IHealthChecksBuilder builder, String WMIClassName, string Column)
        {
            builder.AddCheck($"PerformanceCounter for " + WMIClassName + " Column " + Column, () =>
            {
                try
                {
                    IDictionary<string, Object> data = RunQueryforData("SELECT " + Column + " FROM " + WMIClassName); ////Win32_PerfRawData_PerfOS_Memory
                    data.Add("type", "PerformanceCounter");
                    data.Add("wmiClassName", WMIClassName);
                    data.Add("column", Column);
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    string description = "PerformanceCounter for " + WMIClassName + " Column " + Column;
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (PlatformNotSupportedException ex)
                {
                    return HealthCheckResult.Degraded(ex.Message, ex, new Dictionary<string, object> { { "type", "PerformanceCounter" }, { "wmiClassName", WMIClassName }, { "column", Column } });
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy(ex.Message, ex, new Dictionary<string, object> { { "type", "PerformanceCounter" }, { "wmiClassName", WMIClassName }, { "column", Column } });
                }
            });
            return builder;
        }


        public static IHealthChecksBuilder AddPerformanceCounter(this IHealthChecksBuilder builder, String WMIClassName, params string[] columns)
        {
            string columnsJoined = string.Join(", ", columns);
            builder.AddCheck($"PerformanceCounter for " + WMIClassName + " Columns " + columnsJoined, () =>
            {
                try
                {
                    IDictionary<string, Object> data = RunQueryforData("SELECT " + columnsJoined + " FROM " + WMIClassName);
                    data.Add("type", "PerformanceCounter");
                    data.Add("wmiClassName", WMIClassName);
                    data.Add("column", columnsJoined);
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    string description = $"PerformanceCounter for " + WMIClassName + " Columns " + columnsJoined;
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (PlatformNotSupportedException ex)
                {
                    return HealthCheckResult.Degraded(ex.Message, ex, new Dictionary<string, object> { { "type", "PerformanceCounter" }, { "wmiClassName", WMIClassName }, { "column", columnsJoined } });
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy(ex.Message, ex, new Dictionary<string, object> { { "type", "PerformanceCounter" }, { "wmiClassName", WMIClassName }, { "column", columnsJoined } });
                }
            });
            return builder;
        }

        public static UInt32 RunQueryMB(string query, string properties)
        {
            UInt32 result = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject WniPART in searcher.Get())
            {
                UInt32 SizeinKB = Convert.ToUInt32(WniPART.Properties[properties].Value);
                UInt32 SizeinMB = SizeinKB / 1024;
                result = SizeinMB;
            }
            return result;
        }
        public static object RunQueryMB(string query)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject WniPART in searcher.Get())
            {
                // data = WniPART.Properties;
                foreach (var prop in WniPART.Properties)
                {
                    data.Add(prop.Name, prop.Value);
                }
            }
            return data;
        }
        public static Dictionary<string, object> RunQueryforData(string query)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject WniPART in searcher.Get())
            {
                foreach (var prop in WniPART.Properties)
                {
                    data.Add(prop.Name, prop.Value?.ToString());
                }
            }
            return data;
        }
    }


    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.
    public class SystemInfoHealthChecks : IHealthCheck
    {
        ILogger<SystemInfoHealthChecks> logger;
        public SystemInfoHealthChecks(ILogger<SystemInfoHealthChecks> logger)
        {
            this.logger = logger;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "SystemInfoHealthChecks");
                try { data.Add("PrivateMemorySize (KB)", Process.GetCurrentProcess().PrivateMemorySize64 / 1024); } catch (Exception ex) { data.Add("PrivateMemorySize64", ex.Message); }
                try { data.Add("VirtualMemorySize (KB)", Process.GetCurrentProcess().VirtualMemorySize64 / 1024); } catch (Exception ex) { data.Add("VirtualMemorySize64", ex.Message); }
                try { data.Add("BasePriority", Process.GetCurrentProcess().BasePriority); } catch (Exception ex) { data.Add("BasePriority", ex.Message); }
                // try {data.Add("Container",  Process.GetCurrentProcess().Container);} catch (Exception ex){ data.Add("Container", ex.Message);  }
                try { data.Add("EnableRaisingEvents", Process.GetCurrentProcess().EnableRaisingEvents); } catch (Exception ex) { data.Add("EnableRaisingEvents", ex.Message); }
                try { data.Add("ExitCode", Process.GetCurrentProcess().ExitCode); } catch (Exception ex) { data.Add("ExitCode", ex.Message); }
                try { data.Add("ExitTime", Process.GetCurrentProcess().ExitTime); } catch (Exception ex) { data.Add("ExitTime", ex.Message); }
                // try {data.Add("Handle",  Process.GetCurrentProcess().Handle); } catch (Exception ex){ data.Add("Handle", ex.Message);  }
                try { data.Add("HandleCount", Process.GetCurrentProcess().HandleCount); } catch (Exception ex) { data.Add("HandleCount", ex.Message); }
                try { data.Add("HasExited", Process.GetCurrentProcess().HasExited); } catch (Exception ex) { data.Add("HasExited", ex.Message); }
                try { data.Add("Id", Process.GetCurrentProcess().Id); } catch (Exception ex) { data.Add("Id", ex.Message); }
                try { data.Add("MachineName", Process.GetCurrentProcess().MachineName); } catch (Exception ex) { data.Add("MachineName", ex.Message); }
                // try {data.Add("MainModule",  Process.GetCurrentProcess().MainModule); } catch (Exception ex){ data.Add("MainModule", ex.Message);  }
                //  try {data.Add("MainWindowHandle",  Process.GetCurrentProcess().MainWindowHandle);  } catch (Exception ex){ data.Add("MainWindowHandle", ex.Message);  }
                try { data.Add("MainWindowTitle", Process.GetCurrentProcess().MainWindowTitle); } catch (Exception ex) { data.Add("MainWindowTitle", ex.Message); }
                // try {data.Add("MaxWorkingSet",  Process.GetCurrentProcess().MaxWorkingSet); } catch (Exception ex){ data.Add("MaxWorkingSet", ex.Message);  }
                // try {data.Add("MinWorkingSet",  Process.GetCurrentProcess().MinWorkingSet); } catch (Exception ex){ data.Add("MinWorkingSet", ex.Message);  }
                // try {data.Add("Modules",  Process.GetCurrentProcess().Modules); } catch (Exception ex){ data.Add("Modules", ex.Message);  }
                try { data.Add("NonpagedSystemMemorySize (KB)", Process.GetCurrentProcess().NonpagedSystemMemorySize64 / 1024); } catch (Exception ex) { data.Add("NonpagedSystemMemorySize64", ex.Message); }
                try { data.Add("PagedMemorySize (KB)", Process.GetCurrentProcess().PagedMemorySize64 / 1024); } catch (Exception ex) { data.Add("PagedMemorySize64", ex.Message); }
                try { data.Add("PagedSystemMemorySize (KB)", Process.GetCurrentProcess().PagedSystemMemorySize64 / 1024); } catch (Exception ex) { data.Add("PagedSystemMemorySize64", ex.Message); }
                try { data.Add("PeakPagedMemorySize (KB)", Process.GetCurrentProcess().PeakPagedMemorySize64 / 1024); } catch (Exception ex) { data.Add("PeakPagedMemorySize64", ex.Message); }
                try { data.Add("PeakVirtualMemorySize (KB)", Process.GetCurrentProcess().PeakVirtualMemorySize64 / 1024); } catch (Exception ex) { data.Add("PeakVirtualMemorySize64", ex.Message); }
                try { data.Add("PeakWorkingSet (KB)", Process.GetCurrentProcess().PeakWorkingSet64 / 1024); } catch (Exception ex) { data.Add("PeakWorkingSet64", ex.Message); }
                try { data.Add("PriorityBoostEnabled", Process.GetCurrentProcess().PriorityBoostEnabled); } catch (Exception ex) { data.Add("PriorityBoostEnabled", ex.Message); }
                //  try {data.Add("PriorityClass", Process.GetCurrentProcess().PriorityClass);                                          } catch (Exception ex){ data.Add("PrivateMemorySize64", ex.Message);  }
                try { data.Add("PrivilegedProcessorTime", Process.GetCurrentProcess().PrivilegedProcessorTime); } catch (Exception ex) { data.Add("PrivilegedProcessorTime", ex.Message); }
                try { data.Add("ProcessName", Process.GetCurrentProcess().ProcessName); } catch (Exception ex) { data.Add("ProcessName", ex.Message); }
                // try {data.Add("ProcessorAffinity", Process.GetCurrentProcess().ProcessorAffinity);                                  } catch (Exception ex){ data.Add("PrivateMemorySize64", ex.Message);  }
                try { data.Add("Responding", Process.GetCurrentProcess().Responding); } catch (Exception ex) { data.Add("Responding", ex.Message); }
                // try {data.Add("SafeHandle", Process.GetCurrentProcess().SafeHandle);                                                } catch (Exception ex){ data.Add("PrivateMemorySize64", ex.Message);  }
                try { data.Add("SessionId", Process.GetCurrentProcess().SessionId); } catch (Exception ex) { data.Add("SessionId", ex.Message); }
                try { data.Add("Site", Process.GetCurrentProcess().Site.Name); } catch (Exception ex) { data.Add("Site", ex.Message); }
                //  try {  data.Add("StandardError", Process.GetCurrentProcess().StandardError);  } catch (Exception ex){ data.Add("StandardError", ex.Message);  }
                //  try {  data.Add("StandardInput", Process.GetCurrentProcess().StandardInput);  } catch (Exception ex){ data.Add("StandardInput", ex.Message);  }
                //  try {  data.Add("StandardOutput", Process.GetCurrentProcess().StandardOutput); } catch (Exception ex){ data.Add("StandardOutput", ex.Message);  }
                try { data.Add("StartInfo", Process.GetCurrentProcess().StartInfo.UserName); } catch (Exception ex) { data.Add("StartInfo", ex.Message); }
                //  try {  data.Add("SynchronizingObject", Process.GetCurrentProcess().SynchronizingObject); } catch (Exception ex){ data.Add("SynchronizingObject", ex.Message);  }
                //  try {  data.Add("Threads", Process.GetCurrentProcess().Threads); } catch (Exception ex){ data.Add("Threads", ex.Message);  }
                try { data.Add("TotalProcessorTime", Process.GetCurrentProcess().TotalProcessorTime); } catch (Exception ex) { data.Add("TotalProcessorTime", ex.Message); }
                try { data.Add("UserProcessorTime", Process.GetCurrentProcess().UserProcessorTime); } catch (Exception ex) { data.Add("UserProcessorTime", ex.Message); }
                try { data.Add("WorkingSet (KB)", Process.GetCurrentProcess().WorkingSet64 / 1024); } catch (Exception ex) { data.Add("WorkingSet64", ex.Message); }

                try { data.Add("CPU", GetCpuUsageForProcess()); } catch (Exception ex) { data.Add("CPU", ex.Message); }
                ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                string description = "SystemInfo";
                return HealthCheckResult.Healthy(description, rodata);
            });
        }


        private double GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            var taskwait = Task.Delay(2000);
            taskwait.Wait();

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            logger.LogCritical("cpuUsageTotal : " + cpuUsageTotal + " cpuUsedMs : " + cpuUsedMs.ToString() + " totalMsPassed : " + totalMsPassed.ToString() + " Environment.ProcessorCount : " + Environment.ProcessorCount.ToString());
            return cpuUsageTotal * 100;
        }
    }

}
