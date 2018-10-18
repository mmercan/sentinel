using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Mercan.Common.Mongo;
using Sentinel.Web.Dto.Product;

namespace Sentinel.Web.Api.Product.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/HealthCheck")]
    [ApiExplorerSettings(GroupName = @"Health Check")]
    public class HealthCheckController : Controller
    {

        ILogger<HealthCheckController> _logger;

        public HealthCheckController(ILogger<HealthCheckController> logger, IDistributedCache cache, IOptions<MangoBaseRepoSettings> mangoBaseRepoSettings,
         MangoBaseRepo<ProductInfoDtoV2> repo)
        {
            _logger = logger;
        }

        /// <summary>
        /// isalive
        /// </summary>
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


        /// <summary>
        /// isaliveandwell
        /// </summary>
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
