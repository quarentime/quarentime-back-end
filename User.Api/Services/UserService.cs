using System.Threading.Tasks;
using User.Api.Model;
using System.Linq;
using User.Api.Exceptions;

namespace User.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ICollectionRepository<User.Api.Model.User> _userRepository;
        private readonly ICollectionRepository<PersonalInformation> _personalInformationRepository;
        private readonly ICollectionRepository<SurveyIntake> _surveyRepository;
        private readonly IPhoneVerificationService _phoneVerificationService;

        public UserService(ICollectionRepository<User.Api.Model.User> userRepository,
                           ICollectionRepository<PersonalInformation> personalInformationRepository,
                           ICollectionRepository<SurveyIntake> surveyRepository,
                           IPhoneVerificationService phoneVerificationService)
        {
            _userRepository = userRepository;
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

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                user = new Model.User();
            }

            user.FinalStatus = user.Status = value.Evaluate();
            await _userRepository.UpdateAsync(userId, user);
            return user.Status;
        }

        public async Task<RiskGroup> GetRiskGroupAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return RiskGroup.Healthy;
            }

            return user.Status;
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

        public async Task<PersonalInformation> GetUserByPhone(string phoneNumber)
        {
            return (await _personalInformationRepository
                            .GetByFieldAsync(nameof(PersonalInformation.PhoneNumber), phoneNumber))
                            .FirstOrDefault();
        }
    }
}
