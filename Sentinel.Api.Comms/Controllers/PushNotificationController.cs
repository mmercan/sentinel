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
using Sentinel.Model.PushNotification;
using Mercan.Common.Mongo;

namespace Sentinel.Api.Comms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "azure")]
    public class PushNotificationController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PushNotificationController> _logger;
        private MangoBaseRepo<PushNotificationModel> _mongoRepo;
        public PushNotificationController(IConfiguration configuration, ILogger<PushNotificationController> logger,
        MangoBaseRepo<PushNotificationModel> mongoRepo)
        {
            _config = configuration;
            _logger = logger;
            _mongoRepo = mongoRepo;
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
        public async Task<IActionResult> Post([FromBody] PushNotificationModel model)
        {
            _logger.LogDebug("Post Called!!!");
            var iden = this.User.Identity;
            //var email = this.User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var email = this.User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            model.IdentityEmail = email;
            model.Id = Guid.NewGuid();
            await _mongoRepo.AddAsync(model);
            //TODO: Implement Realistic Implementation
            //var claims = this.User.Claims.Select(p => new { value = p.Value, typee = p.Type }).ToJSON();
            //_logger.LogDebug("iden.Name : " + iden.Name);
            //_logger.LogDebug("claims : " + claims);

            var modelstring = model.ToJSON();
            _logger.LogDebug("model : " + modelstring);

            var payload = JsonConvert.SerializeObject(
              new
              {
                  Email = email,
                  Message = "Welcome",
                  Link = "null"
              }
            );
            _logger.LogDebug(payload);

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
