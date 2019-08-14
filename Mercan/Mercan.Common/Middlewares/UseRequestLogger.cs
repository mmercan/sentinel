using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Microsoft.AspNetCore.Hosting
{
    public static class UseExpireHeaderExtension
    {
        public static IApplicationBuilder UseExpireHeader(this IApplicationBuilder app, TimeSpan expires)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            //Option 1
            //app.Use((context, next) =>
            //{
            //    context.Response.Headers["Cache-Control"] = "private, max-age=43200";
            //    context.Response.Headers["Expires"] = DateTime.UtcNow.Add(Expires).ToString("R");
            //    return next.Invoke();
            //});
            //return app;

            //option 2 for more complex scnerios
            app.UseMiddleware<ExpireHeaderMiddleware>(expires);
            
            return app;
        }


        public class ExpireHeaderMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IHostingEnvironment _hostingEnv;
            private readonly TimeSpan _expires;

            public ExpireHeaderMiddleware(RequestDelegate next, IHostingEnvironment hostingEnv, TimeSpan expires)
            {
                _next = next;
                _hostingEnv = hostingEnv;
                _expires = expires;
                // _logger = loggerFactory.CreateLogger<RequestLoggerMiddleware>();
            }

            public async Task Invoke(HttpContext context)
            {
                
                //context.Response.GetTypedHeaders()
                context.Response.Headers["Cache-Control"] = "private, max-age=43200";
                context.Response.Headers["Expires"] = DateTime.UtcNow.Add(_expires).ToString("R");
                await _next.Invoke(context);
            }
        }
    }

}

