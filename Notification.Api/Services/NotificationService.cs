using Notification.Api.Models;
using Quarentime.Common.Services;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Notification.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfigurationService _configurationService;

        public NotificationService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public Task PushNotification()
        {
            throw new NotImplementedException();
        }

        public async Task SmsAlert(SmsAlertContract request)
        {
            await InitTwilioClient();
            var senderNumber = await _configurationService.GetValue("twilio_sender_number");

            await MessageResource.CreateAsync(
                    body: $"{request.ContactName}'s health status changed to: {request.ContactStatus}",
                    from: new Twilio.Types.PhoneNumber(senderNumber),
                    to: new Twilio.Types.PhoneNumber(request.UserPhoneNumber)
                );
        }

        private async Task InitTwilioClient()
        {
            var accountSid = await _configurationService.GetValue("twilio_sid");
            var authToken = await _configurationService.GetValue("twilio_auth_token");

            TwilioClient.Init(accountSid, authToken);
        }
    }
}
