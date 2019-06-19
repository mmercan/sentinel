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
using System.Globalization;

namespace Mercan.HealthChecks.Common.Checks
{
    public static partial class HealthCheckBuilderExtensions
    {
        // System checks

        public static IHealthChecksBuilder AddPrivateMemorySizeCheck(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"PrivateMemorySize({maxSize.ToString("0,0", CultureInfo.InvariantCulture)})", maxSize, () => Process.GetCurrentProcess().PrivateMemorySize64);

        public static IHealthChecksBuilder AddPrivateMemorySizeCheckKB(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"PrivateMemorySize({maxSize.ToString("0,0", CultureInfo.InvariantCulture)}KB)", maxSize, () => Process.GetCurrentProcess().PrivateMemorySize64 / 1024);
        public static IHealthChecksBuilder AddPrivateMemorySizeCheckMB(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"PrivateMemorySize({maxSize.ToString("0,0", CultureInfo.InvariantCulture)}MB)", maxSize, () => Process.GetCurrentProcess().PrivateMemorySize64 / 1048576);


        public static IHealthChecksBuilder AddVirtualMemorySizeCheck(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"VirtualMemorySize({maxSize.ToString("0,0", CultureInfo.InvariantCulture)})", maxSize, () => Process.GetCurrentProcess().VirtualMemorySize64);

        public static IHealthChecksBuilder AddVirtualMemorySizeCheckKB(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"VirtualMemorySize({maxSize.ToString("0,0", CultureInfo.InvariantCulture)}KB)", maxSize, () => Process.GetCurrentProcess().VirtualMemorySize64 / 1024);

        public static IHealthChecksBuilder AddVirtualMemorySizeCheckMB(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"VirtualMemorySize({maxSize.ToString("0,0", CultureInfo.InvariantCulture)}MB)", maxSize, () => Process.GetCurrentProcess().VirtualMemorySize64 / 1048576);



        public static IHealthChecksBuilder AddWorkingSetCheck(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"WorkingSet({maxSize.ToString("0,0", CultureInfo.InvariantCulture)})", maxSize, () => Process.GetCurrentProcess().WorkingSet64);

        public static IHealthChecksBuilder AddWorkingSetCheckKB(this IHealthChecksBuilder builder, long maxSize)
                 => AddMaxValueCheck(builder, $"WorkingSet({maxSize.ToString("0,0", CultureInfo.InvariantCulture)}KB)", maxSize, () => Process.GetCurrentProcess().WorkingSet64 / 1024);

        public static IHealthChecksBuilder AddWorkingSetCheckMB(this IHealthChecksBuilder builder, long maxSize)
                 => AddMaxValueCheck(builder, $"WorkingSet({maxSize.ToString("0,0", CultureInfo.InvariantCulture)}MB)", maxSize, () => Process.GetCurrentProcess().WorkingSet64 / 1048576);


    }
}
