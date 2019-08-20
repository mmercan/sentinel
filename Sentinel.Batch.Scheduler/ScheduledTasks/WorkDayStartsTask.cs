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
    public class WorkDayStartsTask : IScheduledTask
    {
        private ILogger<WorkDayStartsTask> _logger;
        public WorkDayStartsTask(ILogger<WorkDayStartsTask> logger)
        {
            _logger = logger;
        }
        public string Schedule => "0 8 * * 1-5"; // "At 8:00 on every day-of-week from Monday through Friday;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _logger.LogCritical("WorkDayStartsTask triggered");
            });

        }
    }
}