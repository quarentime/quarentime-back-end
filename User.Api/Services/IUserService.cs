using System.Threading.Tasks;
using User.Api.Model;

namespace User.Api.Services
{
    public interface IUserService
    {
        Task UpdatePersonalInformationAsync(string userId, PersonalInformation value);
        Task<RiskGroup> UpdateSurveyInfo(string userId, SurveyIntake value);
        Task<PersonalInformation> GetPersonalInformationAsync(string userId);
        Task<SurveyIntake> GetSurveyInfoAsync(string userId);
        Task<RiskGroup> GetRiskGroupAsync(string userId);
        Task RequestPhoneValidation(string userId);
        Task<bool> CheckVerificationCode(string userId, string verificationCode);
        Task<PersonalInformation> GetUserByPhone(string phoneNumber);
        Task<ContactTrace> GetUserTraceData(string userId);
        Task<string> GetUserName(string userId);
        Task RegisterUserDeviceToken(string userId, string token);
    }
}
