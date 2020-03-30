using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebPush;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Sentinel.Model.PushNotification;
using Sentinel.Api.Comms.Services;
using Microsoft.FeatureManagement;
using Sentinel.Api.Comms.Models;
using Microsoft.FeatureManagement.Mvc;
using System.Collections.Generic;

namespace Sentinel.Api.Comms.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "azure")]
    public class PushNotificationController : Controller
    {
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<PushNotificationController> _logger;
        private readonly PushNotificationService _pushNotificationService;

        public PushNotificationController(ILogger<PushNotificationController> logger,
        PushNotificationService pushNotificationService, IFeatureManager featureManager)
        {
            _logger = logger;
            _pushNotificationService = pushNotificationService;
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (await _featureManager.IsEnabledAsync(nameof(CommsFeatureFlags.UseQueue)))
            {
                return Content("Queue Used to Blah");
            }
            else
            {
                return Content("Just Blah No Queue involved.");
            }
        }

        [HttpGet("queue")]
        [FeatureGate(CommsFeatureFlags.UseQueue)]
        public async Task<IActionResult> GetQueue()
        {
            return Content("Queue Used to Blah");
        }

        [HttpGet("beta")]
        [FeatureGate(CommsFeatureFlags.Beta)]
        public async Task<IActionResult> GetBeta()
        {
            return Content("Beta Used to Blah");
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

            var pushmessage = new PushNotificationPayloadModel
            {
                Title = "Push Notification registered",
                Email = email,
                Message = "Welcome"
            };

            List<Task> tasks = new List<Task>();
            var task1 = _pushNotificationService.NewUserAsync(email, model);
            var task2 = _pushNotificationService.PushNotification(model, pushmessage);
            tasks.Add(task1);
            tasks.Add(task2);
            Task.WaitAll(tasks.ToArray());
            return Created("", null);
        }

        [HttpPost]
        public async Task PushNotification([FromBody]PushNotificationRequestModel request)
        {
            _logger.LogDebug("looking for " + request.Email);
            await _pushNotificationService.PushNotification(request);
        }

        //generate vapid key from : https://web-push-codelab.glitch.me/

        [HttpPost("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult JustforUsers([FromBody] PushNotificationModel model)
        {
            return Content("Ok");
        }
    }




}
