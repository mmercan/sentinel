// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Diagnostics.HealthChecks;
// using System;
// using System.Collections.Generic;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;

// namespace Mercan.HealthChecks.Common.Checks
// {
//     public static partial class HealthCheckBuilderExtensions
//     {
//         // Default URL check

//         // public static IHealthChecksBuilder AddUrlCheck(this IHealthChecksBuilder builder, string url)
//         // {
//         //     Guard.ArgumentNotNull(nameof(builder), builder);
//         //     return AddUrlCheck(builder, url);
//         // }



//         // Func returning IHealthCheckResu

//         // public static IHealthChecksBuilder AddUrlCheck(this IHealthChecksBuilder builder, string url, Func<HttpResponseMessage, HealthCheckResult> checkFunc)
//         // {
//         //     Guard.ArgumentNotNull(nameof(checkFunc), checkFunc);

//         //     return AddUrlCheck(builder, url,response => new ValueTask<HealthCheckResult>(checkFunc(response)));
//         // }

//         // Func returning Task<IHealthCheckResult>

//         // public static IHealthChecksBuilder AddUrlCheck(this IHealthChecksBuilder builder, string url, Func<HttpResponseMessage, Task<HealthCheckResult>> checkFunc)
//         // {
//         //     Guard.ArgumentNotNull(nameof(builder), builder);

//         //     return AddUrlCheck(builder, url, checkFunc);
//         // }


//         // Func returning ValueTask<IHealthCheckResult>

//         // public static IHealthChecksBuilder AddUrlCheck(this IHealthChecksBuilder builder, string url, Func<HttpResponseMessage, ValueTask<HealthCheckResult>> checkFunc)
//         // {
//         //     Guard.ArgumentNotNull(nameof(builder), builder);

//         //     return AddUrlCheck(builder, url, checkFunc);
//         // }

//     }
// }
