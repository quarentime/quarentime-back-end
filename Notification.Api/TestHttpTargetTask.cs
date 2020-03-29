using Google.Cloud.Tasks.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notification.Api
{
    public static class TestHttpTargetTask
    {
        public static string CreateTask(
            string projectId,
            string location,
            string queue,
            string url,
            string payload,
            int inSeconds = 0)
        {
            CloudTasksClient client = CloudTasksClient.Create();
            QueueName parent = new QueueName(projectId, location, queue);

            var response = client.CreateTask(new CreateTaskRequest
            {
                Parent = parent.ToString(),
                Task = new Task
                {
                    HttpRequest = new HttpRequest
                    {
                        HttpMethod = HttpMethod.Post,
                        Url = url,
                        Body = ByteString.CopyFromUtf8(payload)
                    },
                    ScheduleTime = Timestamp.FromDateTime(DateTime.UtcNow.AddSeconds(inSeconds))
                }
            });

            return response.Name;
        }
    }
}
