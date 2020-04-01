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

        public async Task TwilioNotify(MessageContract contract)
        {
            await InitTwilioClient();
            var senderNumber = await _configurationService.GetValue("twilio_sender_number");

            await MessageResource.CreateAsync(
                    body: contract.Message,
                    from: new Twilio.Types.PhoneNumber(senderNumber),
                    to: new Twilio.Types.PhoneNumber(contract.UserPhoneNumber)
                );
        }

        public async Task PushNotification(MessageContract contract)
        {
            
        }

        private async Task InitTwilioClient()
        {
            var accountSid = await _configurationService.GetValue("twilio_sid");
            var authToken = await _configurationService.GetValue("twilio_auth_token");

            TwilioClient.Init(accountSid, authToken);
        }
    }
}
