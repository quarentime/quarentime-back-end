using Quarentime.Common.Contracts;
using Quarentime.Common.Models;
using System.Threading.Tasks;

namespace Quarentime.Common.Services
{
    public interface ICloudTaskService
    {
        Task SendMessage(MessageContract contract, NotificationType type);
    }
}
