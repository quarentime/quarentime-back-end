using System.Threading.Tasks;
using User.Api.Model;

namespace User.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ICollectionRepository<PersonalInformation> _personalInformationRepository;
        private readonly ICollectionRepository<SurveyIntake> _surveyRepository;
        private readonly IPhoneVerificationService _phoneVerificationService;

        public UserService(ICollectionRepository<PersonalInformation> personalInformationRepository,
                           ICollectionRepository<SurveyIntake> surveyRepository,
                           IPhoneVerificationService phoneVerificationService)
        {
            _personalInformationRepository = personalInformationRepository;
            _surveyRepository = surveyRepository;
            _phoneVerificationService = phoneVerificationService;
        }

        public async Task UpdatePersonalInformationAsync(string userId, PersonalInformation value)
        {
            await _personalInformationRepository.UpdateAsync(userId, value);
        }

        public async Task<RiskGroup> UpdateSurveyInfo(string userId, SurveyIntake value)
        {
            await _surveyRepository.UpdateAsync(userId, value);
            return value.Evaluate();
        }

        public async Task<PersonalInformation> GetPersonalInformationAsync(string userId)
        {
            return await _personalInformationRepository.GetByIdAsync(userId);
        }

        public async Task<SurveyIntake> GetSurveyInfoAsync(string userId)
        {
            return await _surveyRepository.GetByIdAsync(userId);
        }

        public async Task RequestPhoneValidation(string userId)
        {
            var personalInfo = await _personalInformationRepository.GetByIdAsync(userId);
            await _phoneVerificationService.RequestVerificationAsync(userId, personalInfo);
        }

        public async Task<bool> CheckVerificationCode(string userId, string verificationCode)
        {
            // TODO: Add transaction
            if (await _phoneVerificationService.ValidateAsync(userId, verificationCode))
            {
                var personalInformation = await _personalInformationRepository.GetByIdAsync(userId);
                personalInformation.Verified = true;
                await _personalInformationRepository.UpdateAsync(userId, personalInformation);
                return true;
            }
            return false;
        }
    }
}
