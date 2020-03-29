using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notification.Api.Models;
using Notification.Api.Services;

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
        [Route("SmsAlert")]
        public async Task SmsAlert()
        {
            //TODO: Handle http task request sent from cloud tasks api. Should have a working live instance of the api on cloud.
            var request = new SmsAlertRequest { UserPhoneNumber = "+38978691342", ContactName = "Edmar", ContactStatus = "SuspectedCase"};
            await _notificationService.SmsAlert(request);
            //var body = Request.Body;
            //Console.WriteLine(body);
        }
    }
}
