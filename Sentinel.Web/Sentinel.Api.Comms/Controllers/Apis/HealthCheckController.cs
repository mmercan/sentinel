    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    
    namespace Sentinel.Api.Comms.Controllers
    {
        [ApiVersion("1.0", Deprecated = true)]
        [ApiVersion("2.0")]
        [Route("api/HealthCheck")]
        public class HealthCheckController : Controller
        {
    
            ILogger<HealthCheckController> _logger;
    
            public HealthCheckController(ILogger<HealthCheckController> logger)
            {
                _logger = logger;
            }
    
            [HttpGet("isalive")]
            [ProducesResponseType(200)]
            [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
            [ApiExplorerSettings(GroupName = @"HealthCheck")]
            public IActionResult GetIsAlive()
            {
                try
                {
                    return Ok();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to execute isalive");
                    return BadRequest();
                }
            }
    
    
            [HttpGet("isaliveandwell")]
            [ProducesResponseType(200)]
            [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
            [ApiExplorerSettings(GroupName = @"HealthCheck")]
            public IActionResult GetIsAliveAndWell()
            {
                try
                {
                    return Ok();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to execute isaliveandwell");
                    return BadRequest();
                }
            }
    
        }
    }
