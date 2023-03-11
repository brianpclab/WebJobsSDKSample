using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJobsSDKSample
{
    public class WebJobTrigger
    {
        ILogger _logger;
        public WebJobTrigger(ILogger<WebJobTrigger> logger)
        {
            _logger = logger;
        }
        public async Task ExecuteAsync([TimerTrigger("1 * * * * *")] TimerInfo timerInfo )
        {
            if (timerInfo.IsPastDue)
            {
                _logger.LogInformation("Timer is running late!");
                return;
            }
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            await Task.Delay(5000);
        }
    }
}
