using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
// using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Mercan.Common.ScheduledTask;

namespace Sentinel.Handler.Comms.ScheduledTask
{
    // uses https://theysaidso.com/api/
    public class QuoteOfTheDayTask : IScheduledTask
    {
        private ILogger<QuoteOfTheDayTask> _logger;

        public QuoteOfTheDayTask(ILogger<QuoteOfTheDayTask> logger)
        {
            _logger = logger;
        }
        public string Schedule => "*/10 * * * *"; // "*/10 * * * *" //Runs every 10 minutes;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("QuoteOfTheDayTask triggered");
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