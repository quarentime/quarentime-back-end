using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using Quarentime.Common.Contracts;
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
        private readonly IDevicesService _devicesTokenService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IDevicesService devicesTokenService,
            IConfigurationService configurationService,
            ILogger<NotificationService> logger)
        {
            _devicesTokenService = devicesTokenService;
            _configurationService = configurationService;
            _logger = logger;
        }

        public async Task SmsNotification(MessageContract contract)
        {
            await InitTwilioClient();
            var senderNumber = await _configurationService.GetValue(Constants.TWILIO_SENDER_NUMBER);
            try
            {
                var response = await MessageResource.CreateAsync(
                        body: contract.Message,
                        from: new Twilio.Types.PhoneNumber(senderNumber),
                        to: new Twilio.Types.PhoneNumber(contract.UserPhoneNumber)
                    );

                _logger.LogInformation($"Successfuly sent message: {response.Body}");

            }catch(Exception e)
            {
                _logger.LogError(e, $"Failed to send message with Twilio. Exception message: {e.Message}", contract);
            }
        }

        public async Task PushNotification(MessageContract contract)
        {
            var deviceTokens = await _devicesTokenService.GetDeviceRegistrationTokens(contract.UserId);

            var message = new MulticastMessage()
            {
                Tokens = deviceTokens,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Body = contract.Message,
                    Title = contract.Title
                },
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                _logger.LogInformation($"{response.SuccessCount} messages were sent successfully.");

            }catch(FirebaseMessagingException e)
            {
                _logger.LogError(e, $"Failed to send firebase push notification: {e.Message}", contract);
            }
        }

        private async Task InitTwilioClient()
        {
            var accountSid = await _configurationService.GetValue(Constants.TWILIO_SID);
            var authToken = await _configurationService.GetValue(Constants.TWILIO_AUTH_TOKEN);

            TwilioClient.Init(accountSid, authToken);
        }
    }
}
