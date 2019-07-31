using System;

namespace Sentinel.Batch.Scheduler.Controllers
{
    public class PushNotificationModel
    {
        public string Email { get; set; }
        public string Endpoint { get; set; }
        public string ExpirationTime { get; set; }
        public KeyReference Keys { get; set; }
    }

    public class KeyReference
    {
        public string P256dh { get; set; }
        public string Auth { get; set; }

    }
}

