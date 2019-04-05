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
                data.Add("TotalMemoryMB", RunQueryMB("SELECT MaxCapacity FROM Win32_PhysicalMemoryArray", "MaxCapacity"));
                data.Add("Win32_PerfRawData_PerfOS_Memory", RunQueryMB("SELECT * FROM Win32_PerfRawData_PerfOS_Memory"));

                data.Add("PrivateMemorySize64", Process.GetCurrentProcess().PrivateMemorySize64 / 1024);
                data.Add("VirtualMemorySize64", Process.GetCurrentProcess().VirtualMemorySize64 / 1024);
                data.Add("WorkingSet64", Process.GetCurrentProcess().WorkingSet64 / 1024);
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
                IDictionary<string, Object> data = RunQueryforData("SELECT " + columns + " FROM " + WMIClassName);
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
            PropertyDataCollection data = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject WniPART in searcher.Get())
            {
                data = WniPART.Properties;
                //foreach(var prop in WniPART.Properties)
                //{
                //    prop.next
                //}
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
                    data.Add(prop.Name, prop.Value);
                }
            }
            return data;
        }
    }
}
