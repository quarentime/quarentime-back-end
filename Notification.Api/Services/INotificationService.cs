using Quarentime.Common.Contracts;
using System.Threading.Tasks;

namespace Notification.Api.Services
{
    public interface INotificationService
    {
        Task SmsNotification(MessageContract contract);
        Task PushNotification(MessageContract contract);
    }
}
