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
            var request = await FormatRequest(context.Request);
            await _next(context);
            var response = FormatResponse(context.Response, context.TraceIdentifier);
            _logger.LogInformation(message: "{@response} registered", args: response);
            // string responseText = response.ToJSON();
            // _logger.LogInformation(,response);
            // var loggingmessages = LoggerMessage.Define(LogLevel.Information, new EventId(1, "Response"), context.Request.Path);
            // loggingmessages(_logger, null,response);
            // _logger.LogInformation(new EventId(2, "Response"), null, message: responseText, args: response);
            // await responseBody.CopyToAsync(originalBodyStream);
            // }
        }
        private async Task<string> FormatRequest(HttpRequest request)
        {
            //var body = request.Body;
            //request.EnableRewind();
            //var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            //await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //var bodyAsText = Encoding.UTF8.GetString(buffer);
            //request.Body = body;
            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString}";
        }

        private LogHttpResponse FormatResponse(HttpResponse response, string TraceIdentifier = null)
        {
            //response.Body.Seek(0, SeekOrigin.Begin);
            // string text = await new StreamReader(response.Body).ReadToEndAsync();
            // response.Body.Seek(0, SeekOrigin.Begin);

            // var header = response.Headers.ToList().ToJSON();
            var respjson = LogHttpResponse.ToLogHttpResponse(response, TraceIdentifier);
            // var respjson = new { Header = header, ContentLength = response.ContentLength, ContentType = response.ContentType, StatusCode = response.StatusCode }.ToJSON();
            return respjson;

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
        // public IHeaderDictionary Headers { get; set; }
        // public string Body { get; set; }

        // 
        // public IResponseCookies Cookies { get; set; }
        // public bool HasStarted { get; set; }

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
}