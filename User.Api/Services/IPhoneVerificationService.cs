using System.Threading.Tasks;
using User.Api.Model;

namespace User.Api.Services
{
    public interface IPhoneVerificationService
    {
        Task RequestVerificationAsync(string userId, PersonalInformation personalInformation);
        Task<bool> ValidateAsync(string userId, string verificationCode);
    }
}
