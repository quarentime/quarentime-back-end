using System.Threading.Tasks;

namespace User.Api.Configuration
{
    public interface IConfigurationService
    {
        Task<string> GetValue(string configName);
    }
}
