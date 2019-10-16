using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mercan.HealthChecks.Common.Checks
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddDIObjectHealthCheck<T>(this IHealthChecksBuilder builder, Func<T, bool> testfunc)
        {
            var name = typeof(T).Name;
            return builder.AddTypeActivatedCheck<DIObjectChecker<T>>($"DIObjectHealthCheck {name}", null, null, testfunc);
        }
    }

    public class DIObjectChecker<T> : IHealthCheck
    {

        readonly ILogger<DIObjectChecker<T>> _logger;
        readonly T _myObject;
        readonly Func<T, bool> _testfunc;
        public DIObjectChecker(ILogger<DIObjectChecker<T>> logger, T myObject, Func<T, bool> testfunc)
        {
            this._myObject = myObject;
            this._logger = logger;
            _testfunc = testfunc;

            logger.LogCritical("DIHealthCheck Init");
        }



        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "DIHealthCheck");
                try
                {
                    var result = _testfunc(_myObject);
                    if (result)
                    {
                        string description = "DependencyInjection is healthy for all services";
                        ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                        return HealthCheckResult.Healthy(description, rodata);
                    }
                    else
                    {
                        string description = "DependencyInjection is healthy for all services";
                        ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                        return HealthCheckResult.Unhealthy(description, data: rodata);
                    }
                }
                catch (Exception ex)
                {
                    string description = "DependencyInjection is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }
            });
        }

    }
}