using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;

namespace Mercan.HealthChecks.Common
{
    public static partial class WriteResponses
    {

        public static Task WriteListResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("duration", result.TotalDuration),
                new JProperty("results", new JArray(result.Entries.Select(pair =>
                  new JObject(
                       new JProperty("name", pair.Key.ToString()),
                       new JProperty("status", pair.Value.Status.ToString()),
                       new JProperty("description", pair.Value.Description),
                       new JProperty("duration", pair.Value.Duration),
                       new JProperty("type", pair.Value.Data.FirstOrDefault(p => p.Key == "type").Value),
                       new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value)))),
                       new JProperty("exception", pair.Value.Exception?.Message)
                 )
                 ))));
            return httpContext.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
        }

    }
}