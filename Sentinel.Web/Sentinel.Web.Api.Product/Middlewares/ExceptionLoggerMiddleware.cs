using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sentinel.Web.Api.Product
{
    public class ExceptionLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DeveloperExceptionPageOptions _options;
        private readonly ILogger _logger;

        public ExceptionLoggerMiddleware(
            RequestDelegate next,
            IOptions<ExceptionLoggerOptions> options,
            ILoggerFactory loggerFactory
            )
        {

            _logger = loggerFactory.CreateLogger<ExceptionLoggerMiddleware>();
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                var trace = LoggerMessage.Define(LogLevel.Trace, new EventId(2, "Trace"), "An unhandled exception has occurred while executing the request.");
                _logger.LogTrace(new EventId(2, "Trace"), null, "Request-Response " + httpContext.Response.StatusCode.ToString(), httpContext);

            }
            catch (Exception ex)
            {

                var unhandledException = LoggerMessage.Define(LogLevel.Error, new EventId(1, "UnhandledException"), "An unhandled exception has occurred while executing the request.");
                unhandledException(_logger, ex);


                if (httpContext.Response.HasStarted)
                {
                }
                throw;
            }
        }
    }
    public class ExceptionLoggerOptions
    {
        public string Name { get; set; }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggerMiddleware>();
        }
    }
}

