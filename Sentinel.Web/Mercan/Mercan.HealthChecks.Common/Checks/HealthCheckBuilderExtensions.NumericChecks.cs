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
    public static partial class HealthCheckBuilderExtensions
    {
        // Numeric checks

        public static IHealthChecksBuilder AddMinValueCheck<T>(this IHealthChecksBuilder builder, string name, T minValue, Func<T> currentValueFunc) where T : IComparable<T>
        {
            Guard.ArgumentNotNull(nameof(builder), builder);
            Guard.ArgumentNotNullOrEmpty(nameof(name), name);
            Guard.ArgumentNotNull(nameof(currentValueFunc), currentValueFunc);

            builder.AddCheck(name, () =>
            {
                var currentValue = currentValueFunc();
                HealthStatus status = currentValue.CompareTo(minValue) >= 0 ? HealthStatus.Healthy : HealthStatus.Unhealthy;

                return new HealthCheckResult(status, $"min={minValue}, current={currentValue}", null, new Dictionary<string, object> { { "type", "AddMinValueCheck" }, { "min", minValue }, { "current", currentValue } });
            });
            return builder;
        }



        public static IHealthChecksBuilder AddMaxValueCheck<T>(this IHealthChecksBuilder builder, string name, T maxValue, Func<T> currentValueFunc) where T : IComparable<T>
        {
            Guard.ArgumentNotNull(nameof(builder), builder);
            Guard.ArgumentNotNullOrEmpty(nameof(name), name);
            Guard.ArgumentNotNull(nameof(currentValueFunc), currentValueFunc);

            builder.AddCheck(name, () =>
            {
                var currentValue = currentValueFunc();
                var status = currentValue.CompareTo(maxValue) <= 0 ? HealthStatus.Healthy : HealthStatus.Unhealthy;
                return new HealthCheckResult(status, $"max={maxValue}, current={currentValue}", null, new Dictionary<string, object> { { "type", "AddMaxValueCheck" }, { "max", maxValue }, { "current", currentValue } });
            });
            return builder;
        }
    }
}
