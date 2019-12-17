using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebPush;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Sentinel.Model.PushNotification;
using Mercan.Common.Mongo;

namespace Sentinel.Api.Comms.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "azure")]
    public class PushNotificationController : Controller
    {
        private readonly ILogger<PushNotificationController> _logger;
        private readonly MangoBaseRepo<PushNotificationModel> _mongoRepo;
        public PushNotificationController(ILogger<PushNotificationController> logger,
        MangoBaseRepo<PushNotificationModel> mongoRepo)
        {
            _logger = logger;
            _mongoRepo = mongoRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //TODO: Implement Realistic Implementation
            return Content("Blah");
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Post([FromBody] PushNotificationModel model)
        {
            _logger.LogDebug("Post Called!!!");
            var email = this.User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (email == null)
            {
                email = this.User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            }
            model.IdentityEmail = email;
            model.Id = Guid.NewGuid();
            await _mongoRepo.AddAsync(model);

            //TODO: Implement Realistic Implementation
            //var claims = this.User.Claims.Select(p => new { value = p.Value, typee = p.Type }).ToJSON();
            //_logger.LogDebug("claims : " + claims);

            var modelstring = model.ToJSON();
            _logger.LogDebug("model : " + modelstring);

            var payload = JsonConvert.SerializeObject(
              new
              {
                  title = "Push Notification registered",
                  Email = email,
                  Message = "Welcome",
                  Link = "null"
              }
            );
            _logger.LogDebug(payload);

            await NofityUser(model.Endpoint, model.Keys.P256dh, model.Keys.Auth, payload);

            return Created("", null);
        }

        //generate vapid key from : https://web-push-codelab.glitch.me/
        private async Task NofityUser(string endpoint, string p256dh, string auth, string payload)
        {
            var vapidDetails = new VapidDetails(@"mailto:mmercan@outlook.com"
                , "BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA"
                , "r2HJzuoJiFD0uMDoQcKMQCGo8M80wag8kCoTMFf3S34"
              );
            var client = new WebPushClient();
            var subs = new PushSubscription(endpoint, p256dh, auth);

            await client.SendNotificationAsync(subs, payload, vapidDetails);
            //task.Wait();

        }

        [HttpPost("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult JustforUsers([FromBody] PushNotificationModel model)
        {
            return Content("Ok");
        }
    }
}
