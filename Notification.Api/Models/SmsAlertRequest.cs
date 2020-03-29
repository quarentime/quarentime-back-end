namespace Notification.Api.Models
{
    public class SmsAlertRequest
    {
        public string ContactName { get; set; }
        public string ContactStatus { get; set; }
        public string UserPhoneNumber { get; set; }
    }
}