using System;
using System.Linq;
using EasyNetQ;
using Mercan.HealthChecks.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Sentinel.Api.HealthMonitoring.Controllers.Apis
{

    [Route("api/healthcheckpush")]
    public class HealthCheckPushController : Controller
    {

        private readonly ILogger<HealthCheckPushController> _logger;
        private readonly IBus bus;

        public HealthCheckPushController(ILogger<HealthCheckPushController> logger, IBus bus)
        {
            _logger = logger;
            this.bus = bus;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var response = new ListResponse();
                response.Status = "Healthy";
                response.Results = new List<ResultResponse>();

                bus.Publish<ListResponse>(response, "HealthCheck");
                return Ok(response);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to execute GET");
                return BadRequest();
            }
        }
    }
}