using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notification.Api.Models;
using Notification.Api.Services;
using System.Threading.Tasks;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Notification")]
        public async Task NotificationsHandler(MessageContract request)
        {
            await _notificationService.TwilioNotify(request);
        }
    }
}
