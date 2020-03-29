using Quarentime.Common.Repository;
using System.Threading.Tasks;

namespace Quarentime.Common.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ICollectionRepository<ConfigurationSetting> _configurationRepository;

        public ConfigurationService(ICollectionRepository<ConfigurationSetting> configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<string> GetValue(string configName)
        {
            var config = await _configurationRepository.GetByIdAsync(configName);
            if (config == null)
                return null;

            return config.Value;
        }
    }
}
