using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sentinel.Handler.Comms.ScheduledTask
{
    public class SomeOtherTask : IScheduledTask
    {
        private ILogger<SomeOtherTask> _logger;

        public SomeOtherTask(ILogger<SomeOtherTask> logger)
        {
            _logger = logger;
        }
        public string Schedule => "0 5 * * *"; // "*/10 * * * *" //Runs every 10 minutes;
        // public string Schedule => "*/2 * * * *";
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("SomeOtherTask triggered");
            await Task.Delay(5000, cancellationToken);
        }
    }
}