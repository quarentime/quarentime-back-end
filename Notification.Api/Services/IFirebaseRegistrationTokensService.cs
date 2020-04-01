using System.Threading.Tasks;

namespace Notification.Api.Services
{
    public interface IFirebaseRegistrationTokensService
    {
        Task<string> GetDeviceRegistrationToken(string userId);
    }
}
