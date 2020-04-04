using System.Threading.Tasks;

namespace Quarentime.Common.Services
{
    public interface IConfigurationService
    {
        Task<string> GetValue(string configName);
    }
}
