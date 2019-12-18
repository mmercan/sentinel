namespace Sentinel.Model.PushNotification
{
    public class PushNotificationRequestModel
    {
        public string Email { get; set; }
        public PushNotificationPayloadModel Payload { get; set; }
    }
}