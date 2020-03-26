using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using User.Api.Configuration;
using User.Api.Model;

namespace User.Api.Services
{
    public class PhoneVerificationService : IPhoneVerificationService
    {
        private readonly ICollectionRepository<PhoneVerificationCode> _phoneVerificationRepository;
        private readonly IConfigurationService _configurationService;

        public PhoneVerificationService(ICollectionRepository<PhoneVerificationCode> phoneVerificationCode,
                                        IConfigurationService configurationService)
        {
            _phoneVerificationRepository = phoneVerificationCode;
            _configurationService = configurationService;
        }

        public async Task RequestVerificationAsync(string userId, PersonalInformation personalInformation)
        {
            var phoneVerificationCode = new PhoneVerificationCode
            {
                Code = GenerateCode()
            };
            await _phoneVerificationRepository.InsertAsync(userId, phoneVerificationCode);

            await SendSmsAsync(personalInformation.Name,
                                personalInformation.PhoneNumber,
                                phoneVerificationCode.Code);
        }

        private async Task SendSmsAsync(string displayName, string phoneNumber, string code)
        {
            var accountSid = await _configurationService.GetValue("twilio_sid");
            var authToken = await _configurationService.GetValue("twilio_auth_token");
            var senderNumber = await _configurationService.GetValue("twilio_sender_number");

            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
                body: $"Hello {displayName}, this is your Quarentime verification code: {code}",
                from: new Twilio.Types.PhoneNumber(senderNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }

        public async Task<bool> ValidateAsync(string userId, string verificationCode)
        {
            var code = await _phoneVerificationRepository.GetByIdAsync(userId);
            return code != null && code.Code == verificationCode;
        }

        private string GenerateCode()
        {
            var random = new Random();
            var numbers = Enumerable.Range(0, 6).Select(i => random.Next(9)).ToArray();
            return string.Join("", numbers).Insert(3, "-");
        }
    }
}
