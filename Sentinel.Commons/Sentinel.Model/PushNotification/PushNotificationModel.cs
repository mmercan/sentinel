using System;

namespace Sentinel.Model.PushNotification
{
    public class PushNotificationModel
    {
        // public string Email { get; set; }
        public string Endpoint { get; set; }
        public ulong? ExpirationTime { get; set; }
        public KeyReference Keys { get; set; }
    }

    public class KeyReference
    {
        public string P256dh { get; set; }
        public string Auth { get; set; }

    }
}

