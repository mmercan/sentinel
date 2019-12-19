using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebPush;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Sentinel.Model.PushNotification;
using Mercan.Common.Mongo;
using System.Net;
using System.Collections.Generic;
using Sentinel.Api.Comms.Services;

namespace Sentinel.Api.Comms.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "azure")]
    public class PushNotificationController : Controller
    {
        VapidDetails vapidDetails = new VapidDetails(@"mailto:mmercan@outlook.com"
        , "BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA"
        , "r2HJzuoJiFD0uMDoQcKMQCGo8M80wag8kCoTMFf3S34");
        private readonly ILogger<PushNotificationController> _logger;
        private readonly PushNotificationService _pushNotificationService;

        public PushNotificationController(ILogger<PushNotificationController> logger,
        PushNotificationService pushNotificationService)
        {
            _logger = logger;
            _pushNotificationService = pushNotificationService;
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
            await _pushNotificationService.NewUserAsync(email, model);

            // model.IdentityEmail = email;
            // model.Id = Guid.NewGuid();
            // await _mongoRepo.AddAsync(model);

            //TODO: Implement Realistic Implementation
            //var claims = this.User.Claims.Select(p => new { value = p.Value, typee = p.Type }).ToJSON();
            //_logger.LogDebug("claims : " + claims);

            // var modelstring = model.ToJSON();
            // _logger.LogDebug("model : " + modelstring);

            var pushmessage = new PushNotificationPayloadModel
            {
                Title = "Push Notification registered",
                Email = email,
                Message = "Welcome"
            };
            await _pushNotificationService.PushNotification(model, pushmessage);

            // DefaultContractResolver contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            // var payload = JsonConvert.SerializeObject(
            //   new PushNotificationPayloadModel
            //   {
            //       Title = "Push Notification registered",
            //       Email = email,
            //       Message = "Welcome"
            //   }, new JsonSerializerSettings { ContractResolver = contractResolver, Formatting = Formatting.Indented }
            // );
            // _logger.LogDebug(payload);
            // await NofityUser(model, payload);

            return Created("", null);
        }

        [HttpPost]
        public async Task PushNotification([FromBody]PushNotificationRequestModel request)
        {
            _logger.LogDebug("looking for " + request.Email);

            await _pushNotificationService.PushNotification(request);

            // List<Task> listOfTasks = new List<Task>();
            // var items = _mongoRepo.Find(p => p.IdentityEmail == request.Email);

            // DefaultContractResolver contractResolver = new DefaultContractResolver
            // {
            //     NamingStrategy = new CamelCaseNamingStrategy()
            // };

            // var payload = JsonConvert.SerializeObject(
            //   request.Payload, new JsonSerializerSettings { ContractResolver = contractResolver, Formatting = Formatting.Indented });

            // foreach (var item in items)
            // {
            //     listOfTasks.Add(NofityUser(item, payload));
            // }
            // await Task.WhenAll(listOfTasks);
        }

        //generate vapid key from : https://web-push-codelab.glitch.me/
        // private async Task NofityUser(string endpoint, string p256dh, string auth, string payload)
        // {
        //     var client = new WebPushClient();
        //     var subs = new PushSubscription(endpoint, p256dh, auth);
        //     await client.SendNotificationAsync(subs, payload, vapidDetails);
        // }

        // private async Task NofityUser(PushNotificationModel model, string payload)
        // {
        //     var client = new WebPushClient();
        //     var subs = new PushSubscription(model.Endpoint, model.Keys.P256dh, model.Keys.Auth);
        //     try
        //     {
        //         await client.SendNotificationAsync(subs, payload, vapidDetails);
        //         _logger.LogDebug("Message Sent email:" + model.IdentityEmail + payload);
        //     }
        //     catch (WebPushException ex)
        //     {
        //         if (ex.StatusCode == HttpStatusCode.Gone || ex.StatusCode == HttpStatusCode.NotFound)
        //         {
        //             await _mongoRepo.RemoveAsync(model.Id);
        //             _logger.LogError(payload);
        //         }
        //     }
        // }

        [HttpPost("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult JustforUsers([FromBody] PushNotificationModel model)
        {
            return Content("Ok");
        }
    }




}
