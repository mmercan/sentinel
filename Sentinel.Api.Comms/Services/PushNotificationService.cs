using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mercan.Common.Mongo;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sentinel.Model.PushNotification;
using WebPush;

namespace Sentinel.Api.Comms.Services
{
    public class PushNotificationService
    {
        VapidDetails vapidDetails = new VapidDetails(@"mailto:mmercan@outlook.com"
        , "BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA"
        , "r2HJzuoJiFD0uMDoQcKMQCGo8M80wag8kCoTMFf3S34");
        private ILogger<PushNotificationService> _logger;
        private MangoBaseRepo<PushNotificationModel> _mongoRepo;
        public PushNotificationService(ILogger<PushNotificationService> logger, MangoBaseRepo<PushNotificationModel> mongoRepo)
        {
            _logger = logger;
            _mongoRepo = mongoRepo;
        }


        public async Task NewUserAsync(string email, PushNotificationModel model)
        {

            model.IdentityEmail = email;
            model.Id = Guid.NewGuid();
            await _mongoRepo.AddAsync(model);

            var modelstring = model.ToJSON();
            _logger.LogDebug("model : " + modelstring);

        }

        public async Task PushNotification(PushNotificationRequestModel request)
        {
            List<Task> listOfTasks = new List<Task>();
            var items = _mongoRepo.Find(p => p.IdentityEmail == request.Email);

            var payload = payloadToString(request.Payload);
            foreach (var item in items)
            {
                listOfTasks.Add(NofityUser(item, payload));
            }
            await Task.WhenAll(listOfTasks);
        }
        public async Task PushNotification(PushNotificationModel model, PushNotificationPayloadModel payloadmodel)
        {
            string payload = payloadToString(payloadmodel);
            _logger.LogDebug(payload);
            await NofityUser(model, payload);
        }

        private static string payloadToString(PushNotificationPayloadModel payloadmodel)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var payload = JsonConvert.SerializeObject(payloadmodel,
               new JsonSerializerSettings { ContractResolver = contractResolver, Formatting = Formatting.Indented }
            );
            return payload;
        }

        private async Task NofityUser(PushNotificationModel model, string payload)
        {
            var client = new WebPushClient();
            var subs = new PushSubscription(model.Endpoint, model.Keys.P256dh, model.Keys.Auth);
            try
            {
                await client.SendNotificationAsync(subs, payload, vapidDetails);
                _logger.LogDebug("Message Sent email:" + model.IdentityEmail + payload);
            }
            catch (WebPushException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Gone || ex.StatusCode == HttpStatusCode.NotFound)
                {
                    await _mongoRepo.RemoveAsync(model.Id);
                    _logger.LogError(payload);
                }
            }
        }

    }
}