using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sentinel.Api.Comms.Models;
using WebPush;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sentinel.Api.Comms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PushNotificationController> _logger;

        public PushNotificationController(IConfiguration configuration, ILogger<PushNotificationController> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //TODO: Implement Realistic Implementation
            // return Content("Blahhh");
            // return Ok("Blah");
            return Content("Blah");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "azure")]
        public IActionResult Post([FromBody] PushNotificationModel model, [FromHeader]string Email)
        {
            _logger.LogDebug("Post Called");
            var iden = this.User.Identity;
            var email = this.User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            //TODO: Implement Realistic Implementation
            model.Email = Email;
            var payload = JsonConvert.SerializeObject(
              new
              {
                  Email = Email,
                  Message = "Welcome",
                  Link = "null"
              }
            );
            Debug.WriteLine(payload);

            NofityUser(model.Endpoint, model.Keys.P256dh, model.Keys.Auth, payload);
            return Created("", null);
        }


        private void NofityUser(string endpoint, string p256dh, string auth, string payload)
        {
            var vapidDetails = new VapidDetails(@"mailto:mmercan@outlook.com"
                , "BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA"
                , "r2HJzuoJiFD0uMDoQcKMQCGo8M80wag8kCoTMFf3S34"
              );
            var client = new WebPushClient();
            var subs = new PushSubscription(endpoint, p256dh, auth);

            var task = client.SendNotificationAsync(subs, payload, vapidDetails);
            task.Wait();
            var status = task.Status;
        }

        [HttpPost("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult JustforUsers([FromBody] PushNotificationModel model, [FromHeader]string Email)
        {
            return Content("Ok");
        }
    }
}
