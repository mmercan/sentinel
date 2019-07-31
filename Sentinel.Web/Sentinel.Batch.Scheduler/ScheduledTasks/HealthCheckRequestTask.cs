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
    public class HealthCheckRequestTask : IScheduledTask
    {
        private ILogger<HealthCheckRequestTask> _logger;
        public HealthCheckRequestTask(ILogger<HealthCheckRequestTask> logger)
        {
            _logger = logger;
        }
        public string Schedule => "0 */1 * * *"; // "At minute 0 past every hour.;
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("HealtchCheckRequest triggered");
            var httpClient = new HttpClient();
            var quoteJson = JObject.Parse(await httpClient.GetStringAsync("http://quotes.rest/qod.json"));
            QuoteOfTheDay.Current = JsonConvert.DeserializeObject<QuoteOfTheDay>(quoteJson["contents"]["quotes"][0].ToString());
            _logger.LogCritical(QuoteOfTheDay.Current.Quote);
        }
    }

    public class QuoteOfTheDay
    {
        public static QuoteOfTheDay Current { get; set; }
        static QuoteOfTheDay()
        {
            Current = new QuoteOfTheDay { Quote = "No quote", Author = "Maarten" };
        }
        public string Quote { get; set; }
        public string Author { get; set; }
    }
}

