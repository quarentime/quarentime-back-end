using Quarentime.Common.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Model;
using Quarentime.Common.Services;
using Quarentime.Common.Contracts;
using Quarentime.Common.Models;

namespace User.Api.Services
{
    public class PhoneVerificationService : IPhoneVerificationService
    {
        private readonly ICollectionRepository<PhoneVerificationCode> _phoneVerificationRepository;
        private readonly ICloudTaskService _cloudTaskService;

        public PhoneVerificationService(ICollectionRepository<PhoneVerificationCode> phoneVerificationCode,
                                        ICloudTaskService cloudTaskService)
        {
            _cloudTaskService = cloudTaskService;
            _phoneVerificationRepository = phoneVerificationCode;
        }

        public async Task RequestVerificationAsync(string userId, PersonalInformation personalInformation)
        {
            var phoneVerificationCode = new PhoneVerificationCode
            {
                Code = GenerateCode()
            };
            await _phoneVerificationRepository.InsertAsync(userId, phoneVerificationCode);

            var message = new MessageContract
            {
                Message = $"Hello {personalInformation.DisplayName}, this is your Quarentime verification code: {phoneVerificationCode.Code}",
                Title = $"Phone verification code",
                UserId = userId,
                UserPhoneNumber = personalInformation.PhoneNumber
            };

            await _cloudTaskService.SendMessage(message, NotificationType.SmsNotifications);
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
