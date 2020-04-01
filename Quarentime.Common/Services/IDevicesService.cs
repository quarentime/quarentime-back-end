using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quarentime.Common.Services
{
    public interface IDevicesService
    {
        Task<List<string>> GetDeviceRegistrationTokens(string userId);
        Task RegisterDevice(string userId, string token);
    }
}
