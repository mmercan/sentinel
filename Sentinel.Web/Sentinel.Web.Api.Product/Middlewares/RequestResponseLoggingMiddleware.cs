using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sentinel.Web.Api.Product
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            // var originalBodyStream = context.Response.Body;
            // using (var responseBody = new MemoryStream())
            // {
            // context.Response.Body = responseBody;
            LogRequest(context.Request, context.TraceIdentifier);
            await _next(context);
            LogResponse(context.Response, context.TraceIdentifier);

            // string responseText = response.ToJSON();
            // _logger.LogInformation(,response);
            // var loggingmessages = LoggerMessage.Define(LogLevel.Information, new EventId(1, "Response"), context.Request.Path);
            // loggingmessages(_logger, null,response);
            // _logger.LogInformation(new EventId(2, "Response"), null, message: responseText, args: response);
            // await responseBody.CopyToAsync(originalBodyStream);
            // }
        }
        private void LogRequest(HttpRequest requestinfo, string TraceIdentifier = null)
        {
            //var body = request.Body;
            //request.EnableRewind();
            //var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            //await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //var bodyAsText = Encoding.UTF8.GetString(buffer);
            //request.Body = body;

            var request = LogHttpRequest.ToLogHttpRequest(requestinfo, TraceIdentifier);
            _logger.LogInformation(message: "{@requstlog} registered", args: request);
        }

        private void LogResponse(HttpResponse responseinfo, string TraceIdentifier = null)
        {
            //response.Body.Seek(0, SeekOrigin.Begin);
            // string text = await new StreamReader(response.Body).ReadToEndAsync();
            // response.Body.Seek(0, SeekOrigin.Begin);
            // var header = response.Headers.ToList().ToJSON();

            var response = LogHttpResponse.ToLogHttpResponse(responseinfo, TraceIdentifier);
            _logger.LogInformation(message: "{@response} registered", args: response);

        }

    }

    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }


    public class LogHttpResponse
    {
        public List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> Headers { get; set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public int StatusCode { get; set; }
        public string TraceIdentifier { get; set; }

        public static LogHttpResponse ToLogHttpResponse(HttpResponse response, string TraceIdentifier = null)
        {
            var logg = new LogHttpResponse
            {
                Headers = response.Headers.ToList(),
                ContentLength = response.ContentLength,
                ContentType = response.ContentType,
                StatusCode = response.StatusCode,
                TraceIdentifier = TraceIdentifier
            };
            return logg;

        }
    }

    public class LogHttpRequest
    {
        public List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> Headers { get; set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public int StatusCode { get; set; }
        public string TraceIdentifier { get; set; }
        public string Scheme { get; private set; }
        public HostString Host { get; private set; }
        public PathString Path { get; private set; }
        public QueryString QueryString { get; private set; }

        public static LogHttpRequest ToLogHttpRequest(HttpRequest request, string TraceIdentifier = null)
        {
            var logg = new LogHttpRequest
            {
                Scheme = request.Scheme,
                Host = request.Host,
                Path = request.Path,
                QueryString = request.QueryString,

                Headers = request.Headers.ToList(),
                ContentLength = request.ContentLength,
                ContentType = request.ContentType,

                TraceIdentifier = TraceIdentifier
            };
            return logg;



        }
    }
}