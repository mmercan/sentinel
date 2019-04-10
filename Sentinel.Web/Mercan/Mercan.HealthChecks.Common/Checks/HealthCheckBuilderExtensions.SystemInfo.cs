using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management;
using System.Text;

namespace Mercan.HealthChecks.Common.Checks
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddSystemInfoCheck(this IHealthChecksBuilder builder)
        {
            builder.AddCheck($"SystemInfoCheck", () =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                // data.Add("TotalMemoryMB", RunQueryMB("SELECT MaxCapacity FROM Win32_PhysicalMemoryArray", "MaxCapacity"));
                // data.Add("Win32_PerfRawData_PerfOS_Memory", RunQueryMB("SELECT * FROM Win32_PerfRawData_PerfOS_Memory"));

                 try {data.Add("PrivateMemorySize64", Process.GetCurrentProcess().PrivateMemorySize64 / 1024); } catch (Exception ex){ data.Add("PrivateMemorySize64", ex.Message);  }
                 try {data.Add("VirtualMemorySize64", Process.GetCurrentProcess().VirtualMemorySize64 / 1024); } catch (Exception ex){ data.Add("VirtualMemorySize64", ex.Message);  }
                 try {data.Add("BasePriority", Process.GetCurrentProcess().BasePriority);} catch (Exception ex){ data.Add("BasePriority", ex.Message);  }
                // try {data.Add("Container",  Process.GetCurrentProcess().Container);} catch (Exception ex){ data.Add("Container", ex.Message);  }
                 try {data.Add("EnableRaisingEvents",  Process.GetCurrentProcess().EnableRaisingEvents); } catch (Exception ex){ data.Add("EnableRaisingEvents", ex.Message);  }
                 try {data.Add("ExitCode",  Process.GetCurrentProcess().ExitCode); } catch (Exception ex){ data.Add("ExitCode", ex.Message);  }
                 try {data.Add("ExitTime",  Process.GetCurrentProcess().ExitTime); } catch (Exception ex){ data.Add("ExitTime", ex.Message);  }
                // try {data.Add("Handle",  Process.GetCurrentProcess().Handle); } catch (Exception ex){ data.Add("Handle", ex.Message);  }
                 try {data.Add("HandleCount",  Process.GetCurrentProcess().HandleCount); } catch (Exception ex){ data.Add("HandleCount", ex.Message);  }
                 try {data.Add("HasExited",  Process.GetCurrentProcess().HasExited); } catch (Exception ex){ data.Add("HasExited", ex.Message);  }
                 try {data.Add("Id",  Process.GetCurrentProcess().Id); } catch (Exception ex){ data.Add("Id", ex.Message);  }
                 try {data.Add("MachineName",  Process.GetCurrentProcess().MachineName); } catch (Exception ex){ data.Add("MachineName", ex.Message);  }
                // try {data.Add("MainModule",  Process.GetCurrentProcess().MainModule); } catch (Exception ex){ data.Add("MainModule", ex.Message);  }
               //  try {data.Add("MainWindowHandle",  Process.GetCurrentProcess().MainWindowHandle);  } catch (Exception ex){ data.Add("MainWindowHandle", ex.Message);  }
                 try {data.Add("MainWindowTitle",  Process.GetCurrentProcess().MainWindowTitle); } catch (Exception ex){ data.Add("MainWindowTitle", ex.Message);  }
                // try {data.Add("MaxWorkingSet",  Process.GetCurrentProcess().MaxWorkingSet); } catch (Exception ex){ data.Add("MaxWorkingSet", ex.Message);  }
                // try {data.Add("MinWorkingSet",  Process.GetCurrentProcess().MinWorkingSet); } catch (Exception ex){ data.Add("MinWorkingSet", ex.Message);  }
                // try {data.Add("Modules",  Process.GetCurrentProcess().Modules); } catch (Exception ex){ data.Add("Modules", ex.Message);  }
                 try {data.Add("NonpagedSystemMemorySize64", Process.GetCurrentProcess().NonpagedSystemMemorySize64 / 1024); } catch (Exception ex){ data.Add("NonpagedSystemMemorySize64", ex.Message);  }
                 try {data.Add("PagedMemorySize64", Process.GetCurrentProcess().PagedMemorySize64 / 1024); } catch (Exception ex){ data.Add("PagedMemorySize64", ex.Message);  }
                 try {data.Add("PagedSystemMemorySize64", Process.GetCurrentProcess().PagedSystemMemorySize64 / 1024); } catch (Exception ex){ data.Add("PagedSystemMemorySize64", ex.Message);  }
                 try {data.Add("PeakPagedMemorySize64", Process.GetCurrentProcess().PeakPagedMemorySize64 / 1024);} catch (Exception ex){ data.Add("PeakPagedMemorySize64", ex.Message);  }
                 try {data.Add("PeakVirtualMemorySize64", Process.GetCurrentProcess().PeakVirtualMemorySize64 / 1024); } catch (Exception ex){ data.Add("PeakVirtualMemorySize64", ex.Message);  }
                 try {data.Add("PeakWorkingSet64", Process.GetCurrentProcess().PeakWorkingSet64 / 1024); } catch (Exception ex){ data.Add("PeakWorkingSet64", ex.Message);  }
                 try {data.Add("PriorityBoostEnabled", Process.GetCurrentProcess().PriorityBoostEnabled); } catch (Exception ex){ data.Add("PriorityBoostEnabled", ex.Message);  }
               //  try {data.Add("PriorityClass", Process.GetCurrentProcess().PriorityClass);                                          } catch (Exception ex){ data.Add("PrivateMemorySize64", ex.Message);  }
                 try {data.Add("PrivilegedProcessorTime", Process.GetCurrentProcess().PrivilegedProcessorTime);} catch (Exception ex){ data.Add("PrivilegedProcessorTime", ex.Message);  }
                 try {data.Add("ProcessName", Process.GetCurrentProcess().ProcessName); } catch (Exception ex){ data.Add("ProcessName", ex.Message);  }
                // try {data.Add("ProcessorAffinity", Process.GetCurrentProcess().ProcessorAffinity);                                  } catch (Exception ex){ data.Add("PrivateMemorySize64", ex.Message);  }
                 try {data.Add("Responding", Process.GetCurrentProcess().Responding);  } catch (Exception ex){ data.Add("Responding", ex.Message);  }
                // try {data.Add("SafeHandle", Process.GetCurrentProcess().SafeHandle);                                                } catch (Exception ex){ data.Add("PrivateMemorySize64", ex.Message);  }
                 try {data.Add("SessionId", Process.GetCurrentProcess().SessionId); } catch (Exception ex){ data.Add("SessionId", ex.Message);  }
                 try {  data.Add("Site", Process.GetCurrentProcess().Site.Name);  } catch (Exception ex){ data.Add("Site", ex.Message);  }
               //  try {  data.Add("StandardError", Process.GetCurrentProcess().StandardError);  } catch (Exception ex){ data.Add("StandardError", ex.Message);  }
               //  try {  data.Add("StandardInput", Process.GetCurrentProcess().StandardInput);  } catch (Exception ex){ data.Add("StandardInput", ex.Message);  }
               //  try {  data.Add("StandardOutput", Process.GetCurrentProcess().StandardOutput); } catch (Exception ex){ data.Add("StandardOutput", ex.Message);  }
                 try {  data.Add("StartInfo", Process.GetCurrentProcess().StartInfo.UserName); } catch (Exception ex){ data.Add("StartInfo", ex.Message);  }
               //  try {  data.Add("SynchronizingObject", Process.GetCurrentProcess().SynchronizingObject); } catch (Exception ex){ data.Add("SynchronizingObject", ex.Message);  }
               //  try {  data.Add("Threads", Process.GetCurrentProcess().Threads); } catch (Exception ex){ data.Add("Threads", ex.Message);  }
                 try {  data.Add("TotalProcessorTime", Process.GetCurrentProcess().TotalProcessorTime); } catch (Exception ex){ data.Add("TotalProcessorTime", ex.Message);  }
                 try {  data.Add("UserProcessorTime", Process.GetCurrentProcess().UserProcessorTime); } catch (Exception ex){ data.Add("UserProcessorTime", ex.Message);  }
                 try { data.Add("WorkingSet64", Process.GetCurrentProcess().WorkingSet64 / 1024); } catch (Exception ex) { data.Add("WorkingSet64", ex.Message); }

                ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                string description = "SystemInfoCheck";
                return HealthCheckResult.Healthy(description, rodata);
            });
            return builder;
        }


        public static IHealthChecksBuilder AddPerformanceCounter(this IHealthChecksBuilder builder, String WMIClassName)
        {
            builder.AddCheck($"PerformanceCounter for " + WMIClassName, () =>
            {
                IDictionary<string, Object> data = RunQueryforData("SELECT * FROM " + WMIClassName); //Win32_PerfRawData_PerfOS_Memory
                ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                string description = "PerformanceCounter for " + WMIClassName;
                return HealthCheckResult.Healthy(description, rodata);
            });
            return builder;
        }

        public static IHealthChecksBuilder AddPerformanceCounter(this IHealthChecksBuilder builder, String WMIClassName, string Column)
        {
            builder.AddCheck($"PerformanceCounter for " + WMIClassName + " Column " + Column, () =>
            {
                //IDictionary<string, Object> data = new Dictionary<string, object>();
                //// data.Add("TotalMemoryMB", RunQueryMB("SELECT MaxCapacity FROM Win32_PhysicalMemoryArray", "MaxCapacity"));
                //data.Add(WMIClassName, RunQueryMB("SELECT "+Column+" FROM "+WMIClassName)); ////Win32_PerfRawData_PerfOS_Memory

                IDictionary<string, Object> data = RunQueryforData("SELECT " + Column + " FROM " + WMIClassName); ////Win32_PerfRawData_PerfOS_Memory
                ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                string description = "PerformanceCounter for " + WMIClassName + " Column " + Column;
                return HealthCheckResult.Healthy(description, rodata);
            });
            return builder;
        }


        public static IHealthChecksBuilder AddPerformanceCounter(this IHealthChecksBuilder builder, String WMIClassName, params string[] columns)
        {
            string columnsJoined = string.Join(",", columns);
            builder.AddCheck($"PerformanceCounter for " + WMIClassName + " Columns " + columnsJoined, () =>
            {
                //IDictionary<string, Object> data = new Dictionary<string, object>();
                //data.Add("TotalMemoryMB", RunQueryMB("SELECT MaxCapacity FROM Win32_PhysicalMemoryArray", "MaxCapacity"));
                IDictionary<string, Object> data = RunQueryforData("SELECT " + columnsJoined + " FROM " + WMIClassName);
                ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                string description = $"PerformanceCounter for " + WMIClassName + " Columns " + columnsJoined;
                return HealthCheckResult.Healthy(description, rodata);
            });
            return builder;
        }

        //public static IHealthChecksBuilder AddProcessList(this IHealthChecksBuilder builder, TimeSpan? cacheDuration = null)
        //{
        //    builder.AddCheck($"AddProcessList", () =>
        //    {
        //        IDictionary<string, Object> data = new Dictionary<string, object>();
        //        UInt32 totalsize = 0;
        //        int number = 0;
        //        foreach (var aProc in Process.GetProcesses())
        //        {
        //            number++;
        //            data.Add(aProc.ProcessName + " " + number.ToString(), aProc.WorkingSet64 / 1024);
        //            totalsize += Convert.ToUInt32(aProc.WorkingSet64 / 1024.0);
        //        }

        //        data.Add("Total Memory", totalsize);
        //        ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
        //        string description = "ProcessList Total Memory Usage " + totalsize + " MB";
        //        return HealthCheckResult.Healthy(description, rodata);
        //    }, cacheDuration ?? builder.DefaultCacheDuration);
        //    return builder;
        //}


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

        //public static string RunQueryMB(string query)
        //{
        //    string serialisedContent = "";
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        //    foreach (ManagementObject WniPART in searcher.Get())
        //    {
        //        serialisedContent += JsonConvert.SerializeObject(WniPART.Properties);
        //    }
        //    return serialisedContent;
        //}

        public static object RunQueryMB(string query)
        {
            //List<PropertyDataCollection> data = new List<PropertyDataCollection>();
            //PropertyDataCollection data = null;
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
}
