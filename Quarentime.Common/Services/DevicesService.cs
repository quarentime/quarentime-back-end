using Quarentime.Common.Models;
using Quarentime.Common.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quarentime.Common.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ISubCollectionRepository<Device> _devicesRepository;

        public DevicesService(ISubCollectionRepository<Device> devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        public async Task<List<string>> GetDeviceRegistrationTokens(string userId)
        {
            return (await _devicesRepository.GetAllAsync(userId)).Select(x => x.Token).ToList();
        }
       
        public async Task RegisterDevice(string userId, string token)
        {
            await _devicesRepository.InsertAsync(userId, new Device(token));
        }
    }
}
