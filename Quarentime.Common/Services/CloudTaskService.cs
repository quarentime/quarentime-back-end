using Google.Protobuf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quarentime.Common.Contracts;
using Quarentime.Common.Models;
using System;
using System.Threading.Tasks;
using gct = Google.Cloud.Tasks.V2;

namespace Quarentime.Common.Services
{
    public class CloudTaskService : ICloudTaskService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CloudTaskService> _logger;

        public CloudTaskService(ILogger<CloudTaskService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendMessage(MessageContract contract, NotificationType type)
        {
            var client = await gct.CloudTasksClient.CreateAsync();
            var tuple = GetQueueByType(type);

            var queueName = _configuration.GetSection(tuple.Item1).Value;
            var location = _configuration.GetSection(Constants.LOCATION).Value;
            var projectId = _configuration.GetSection(Constants.PROJECT_ID).Value;
            var serviceUrl = _configuration.GetSection(Constants.NOTIFICATION_SERVICE_URL).Value;

            gct.QueueName queue = new gct.QueueName(projectId, location, queueName);

            try
            {
                client.CreateTask(new gct.CreateTaskRequest
                {
                    Parent = queue.ToString(),
                    Task = new gct.Task
                    {
                        HttpRequest = new gct.HttpRequest
                        {
                            HttpMethod = gct.HttpMethod.Post,
                            Body = ByteString.CopyFromUtf8(JsonConvert.SerializeObject(contract)),
                            Url = $"{serviceUrl}{tuple.Item2}"
                        }
                    }
                });
            }catch(Exception e)
            {
                _logger.LogError(e, e.Message, queue.ToString());
            }

        }

        private (string, string) GetQueueByType(NotificationType type) => type switch
        {
            NotificationType.PushNotifications => (Constants.PUSH_NOTIFICATIONS_QUEUE, "/notification/push"),
            NotificationType.SmsNotifications => (Constants.SMS_NOTIFICATIONS_QUEUE, "/notification/sms"),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
