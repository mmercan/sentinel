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
using System.Management;

namespace Mercan.HealthChecks.Common.Checks
{
    // Simulates a health check for an application dependency that takes a while to initialize.
    // This is part of the readiness/liveness probe sample.
    public class AddPerformanceCounter : IHealthCheck
    {
        private string wmiClassName;
        private string column;
        private string[] columns;

        public AddPerformanceCounter(string wmiClassName)
        {
            this.wmiClassName = wmiClassName;
        }

        public AddPerformanceCounter(string wmiClassName, string column)
        {
            this.wmiClassName = wmiClassName;
            this.column = column;
        }


        public AddPerformanceCounter(string wmiClassName, params string[] columns)
        {
            this.wmiClassName = wmiClassName;
            this.columns = columns;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            IDictionary<string, Object> data = null;
            string description = null;
            if (columns != null)
            {
                string columnsJoined = string.Join(",", columns);
                data = RunQueryforData("SELECT " + columnsJoined + " FROM " + wmiClassName);
                description = $"PerformanceCounter for " + wmiClassName + " Columns " + columnsJoined;

            }
            else if (!String.IsNullOrWhiteSpace(column))
            {
                data = RunQueryforData("SELECT " + column + " FROM " + wmiClassName);
                description = "PerformanceCounter for " + wmiClassName + " Column " + column;
            }
            else
            {
                data = RunQueryforData("SELECT * FROM " + wmiClassName); //Win32_PerfRawData_PerfOS_Memory
                description = "PerformanceCounter for " + wmiClassName;
            }


            ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
            return HealthCheckResult.Healthy(description, rodata);

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