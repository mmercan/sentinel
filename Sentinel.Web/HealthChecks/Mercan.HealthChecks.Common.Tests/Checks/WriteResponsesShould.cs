
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class WriteResponsesShould
    {
        //HealthCheckContext context = new HealthCheckContext();
        HttpContext context;
        Task<HealthReport> reportTask;
        public WriteResponsesShould()
        {

            context = new DefaultHttpContext();


            var services = new ServiceCollection()
            .AddLogging();

            services.AddHealthChecks()
            .AddMinValueCheck("testitMin", 500, () => { return 300; })
            .AddMaxValueCheck("testitMax", 500, () => { return 900; });


            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<DIHealthCheck>();
            var healthChecksBuilder = serviceProvider.GetService<IHealthChecksBuilder>();
            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            reportTask = healthCheckService.CheckHealthAsync();





        }
        [Fact]
        public async Task WriteDictionaryResponse()
        {
            var report = await reportTask;
            await WriteResponses.WriteDictionaryResponse(context, report);

        }


        [Fact]
        public async Task WriteResponse()
        {
            var report = await reportTask;
            await WriteResponses.WriteResponse(context, report);
        }


        [Fact]
        public async Task WriteListResponse()
        {
            var report = await reportTask;
            await WriteResponses.WriteListResponse(context, report);
        }

    }
}