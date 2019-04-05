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
    public static partial class HealthCheckBuilderExtensions
    {
        // System checks

        public static IHealthChecksBuilder AddPrivateMemorySizeCheck(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"PrivateMemorySize({maxSize})", maxSize, () => Process.GetCurrentProcess().PrivateMemorySize64);


        public static IHealthChecksBuilder AddVirtualMemorySizeCheck(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"VirtualMemorySize({maxSize})", maxSize, () => Process.GetCurrentProcess().VirtualMemorySize64);


        public static IHealthChecksBuilder AddWorkingSetCheck(this IHealthChecksBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"WorkingSet({maxSize})", maxSize, () => Process.GetCurrentProcess().WorkingSet64);


    }
}
