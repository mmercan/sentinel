using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using Mercan.Common.Mail;

namespace Sentinel.UI.Sts.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/HealthCheck")]
    [ApiExplorerSettings(GroupName = @"Health Check")]
    public class HealthCheckController : Controller
    {

        ILogger<HealthCheckController> _logger;
        private MailService mailService;

        public HealthCheckController(ILogger<HealthCheckController> logger, MailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
            // var client = new SmtpClient("smtp.gmail.com", 587);


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
                _logger.LogDebug("GetIsAliveAndWell Called");
                mailService.Send("fff@hhh.com", "blah", "blh");
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
