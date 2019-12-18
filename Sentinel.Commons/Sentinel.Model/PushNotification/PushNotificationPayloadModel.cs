using System.Collections.Generic;

namespace Sentinel.Model.PushNotification
{
    public class PushNotificationPayloadModel
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public List<PushNotificationPayloadLinkModel> Links { get; set; }
    }
}