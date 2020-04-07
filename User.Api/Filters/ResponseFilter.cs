using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace User.Api.Filters
{
    public class ResponseFilter : IResultFilter
    {
        private readonly ILogger<ResponseFilter> _logger;
        
        public ResponseFilter(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<ResponseFilter>();
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var response = context.Result as JsonResult;

            _logger.LogInformation($"Response: {JsonConvert.SerializeObject(response)}");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            
        }
    }
}
