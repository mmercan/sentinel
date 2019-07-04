using System;
using System.Linq;
using EasyNetQ;
using Mercan.HealthChecks.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mercan.HealthChecks.Common.CheckCaller;
using System.Threading;

namespace Sentinel.Api.HealthMonitoring.Controllers.Apis
{

    [Route("api/HealthCheckDownloaderController")]
    public class HealthCheckDownloaderController : Controller
    {

        readonly ILogger<HealthCheckDownloaderController> _logger;
        readonly private HealthCheckReportDownloaderService healthCheckReportDownloaderService;

        public HealthCheckDownloaderController(ILogger<HealthCheckDownloaderController> logger, HealthCheckReportDownloaderService healthCheckReportDownloaderService)
        {
            _logger = logger;
            this.healthCheckReportDownloaderService = healthCheckReportDownloaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string url)
        {
            try
            {
                var content = await healthCheckReportDownloaderService.DownloadAsync(url);
                return Ok(content);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to execute GET");
                return BadRequest();
            }
        }
    }
}