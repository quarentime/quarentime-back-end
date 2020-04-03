﻿using Google.Api.Gax.Grpc;
using Google.Protobuf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
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
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<CloudTaskService> _logger;

        public CloudTaskService(ILogger<CloudTaskService> logger, 
            IConfiguration configuration,
            IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendMessage(MessageContract contract, NotificationType type)
        {
            var client = await gct.CloudTasksClient.CreateAsync();
            var tuple = GetQueueByType(type);

            var queueName = _configuration.GetSection(tuple.queue).Value;
            var location = _configuration.GetSection(Constants.LOCATION).Value;
            var projectId = _configuration.GetSection(Constants.PROJECT_ID).Value;
            var serviceUrl = await _configurationService.GetValue(Constants.NOTIFICATION_SERVICE_URL);

            gct.QueueName queue = new gct.QueueName(projectId, location, queueName);

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
            };
            jsonSettings.Converters.Add(new StringEnumConverter(new SnakeCaseNamingStrategy()));

            try
            {
                var request = new gct.HttpRequest
                {
                    HttpMethod = gct.HttpMethod.Post,
                    Body = ByteString.CopyFromUtf8(JsonConvert.SerializeObject(contract, jsonSettings)),
                    Url = $"{serviceUrl}{tuple.resourceUrl}"
                };
                request.Headers.Add("Content-Type", "application/json");

                var task = new gct.CreateTaskRequest
                {
                    Parent = queue.ToString(),
                    Task = new gct.Task
                    {
                        HttpRequest = request
                    }
                };
                
                await client.CreateTaskAsync(task, CallSettings.FromHeader("Content-Type", "application/json"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, queue.ToString());
            }

        }

        private (string queue, string resourceUrl) GetQueueByType(NotificationType type) => type switch
        {
            NotificationType.PushNotifications => (Constants.PUSH_NOTIFICATIONS_QUEUE, "/notification/push"),
            NotificationType.SmsNotifications => (Constants.SMS_NOTIFICATIONS_QUEUE, "/notification/sms"),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
