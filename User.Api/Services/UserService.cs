using System.Threading.Tasks;
using User.Api.Model;

namespace User.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ICollectionRepository<PersonalInformation> _personalInformationRepository;
        private readonly ICollectionRepository<SurveyIntake> _surveyRepository;

        public UserService(ICollectionRepository<PersonalInformation> personalInformationRepository,
                            ICollectionRepository<SurveyIntake> surveyRepository)
        {
            _personalInformationRepository = personalInformationRepository;
            _surveyRepository = surveyRepository;
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
    }
}
