using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mercan.HealthChecks.Common
{
    public static partial class WriteResponses
    {
        public static Task WriteDictionaryResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            //As Dictionary
            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("duration", result.TotalDuration),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("duration", pair.Value.Duration),

                        new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value)))),
                        new JProperty("exception", pair.Value.Exception?.Message) //new JObject(pair.Value.Exception.Select(p => new JProperty(p.Key, p.Value))))
                                                    ))))));
            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));

        }

    }


    public class DictionaryResponse
    {
        public string Status { get; set; }
        public string Duration { get; set; }
        public Dictionary<string, ResultResponse> Results { get; set; }
    }

    public class ListResponse
    {
        public string Status { get; set; }
        public string Duration { get; set; }
        public List<ResultResponse> Results { get; set; }
    }

    public class ResultResponse
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public string Exception { get; set; }
    }

}