namespace Notification.Api.Models
{
    public class QueueMessage<T> where T : class
    {
        public T Payload { get; set; }
    }
}
