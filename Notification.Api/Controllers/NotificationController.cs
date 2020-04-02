using Microsoft.AspNetCore.Mvc;
using Notification.Api.Services;
using Quarentime.Common.Contracts;
using System.Threading.Tasks;
using Quarentime.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger, 
            INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("Push")]
        public async Task PushNotification([FromBody]MessageContract contract)
        {
            _logger.LogInformation(contract.Message);
            await _notificationService.PushNotification(contract);
        }

        [HttpPost]
        [Route("Sms")]
        public async Task SmsNotification([FromBody]MessageContract contract)
        {
            await _notificationService.SmsNotification(contract);
        }
    }
}
