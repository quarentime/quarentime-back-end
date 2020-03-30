using Notification.Api.Models;
using System.Threading.Tasks;

namespace Notification.Api.Services
{
    public interface INotificationService
    {
        Task PushNotification();
        Task SmsAlert(SmsAlertContract request);
    }
}
