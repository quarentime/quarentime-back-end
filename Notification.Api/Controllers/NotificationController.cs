using Microsoft.AspNetCore.Mvc;
using Notification.Api.Services;
using Quarentime.Common.Contracts;
using System.Threading.Tasks;
using Quarentime.Common.Helpers;
using Newtonsoft.Json;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("Push")]
        public async Task PushNotification()
        {
            var message = JsonConvert.DeserializeObject<MessageContract>(Request.Body.StreamToString());
            await _notificationService.PushNotification(message);
        }

        [HttpPost]
        [Route("Sms")]
        public async Task SmsNotification()
        {
            var message = JsonConvert.DeserializeObject<MessageContract>(Request.Body.StreamToString());
            await _notificationService.SmsNotification(message);
        }
    }
}
