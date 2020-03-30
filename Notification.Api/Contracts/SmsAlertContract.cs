namespace Notification.Api.Models
{
    public class SmsAlertContract
    {
        public string ContactName { get; set; }
        public string ContactStatus { get; set; }
        public string UserPhoneNumber { get; set; }
    }
}