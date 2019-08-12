using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Mercan.Common.AspNetCore.Middlewares
{
    public class AllAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AllAuthenticationMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes, ILogger<AllAuthenticationMiddleware> logger)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (schemes == null)
            {
                throw new ArgumentNullException(nameof(schemes));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            this.logger = logger;
            _next = next;
            Schemes = schemes;
        }

        public AllAuthenticationMiddleware(IAuthenticationSchemeProvider schemes)
        {
            this.Schemes = schemes;

        }
        public IAuthenticationSchemeProvider Schemes { get; set; }
        ILogger<AllAuthenticationMiddleware> logger;

        public async Task Invoke(HttpContext context)
        {
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });

            // Give any IAuthenticationRequestHandler schemes a chance to handle the request
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetAllSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(context, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    return;
                }
                var result = await context.AuthenticateAsync(scheme.Name);
                if (result != null && result.Principal != null && context != null && result.Principal is ClaimsPrincipal)
                {
                    var user = context.User;
                    var principal = result.Principal;
                    user = principal;
                }
            }
            await _next(context);

            //foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            // foreach (var scheme in await Schemes.GetAllSchemesAsync())
            // {
            //      logger.LogCritical("scheme");
            //     var handler = await handlers.GetHandlerAsync(context, scheme.Name) as IAuthenticationRequestHandler;
            //     if (handler != null && await handler.HandleRequestAsync())
            //     {
            //         return;
            //     }
            // }
            // var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            // if (defaultAuthenticate != null)
            // {
            //     var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
            //     if (result?.Principal != null)
            //     {
            //         context.User = result.Principal;
            //     }
            // }           
        }
    }
}