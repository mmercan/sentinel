using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Mercan.Common.ScheduledTask;


namespace Sentinel.Batch.Scheduler.ScheduledTasks
{
    public class WorkDayEndsTask : IScheduledTask
    {
        private ILogger<WorkDayEndsTask> _logger;
        public WorkDayEndsTask(ILogger<WorkDayEndsTask> logger)
        {
            _logger = logger;
        }
        public string Schedule => "0 17 * * 1-5"; // "At 17:00 on every day-of-week from Monday through Friday;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("WorkDayEndsTask triggered");
        }
    }
}