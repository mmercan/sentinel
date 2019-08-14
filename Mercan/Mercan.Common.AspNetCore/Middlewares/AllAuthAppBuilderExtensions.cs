using System;
using Microsoft.AspNetCore.Authentication;
using Mercan.Common.AspNetCore;
using Mercan.Common.AspNetCore.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class AuthAppBuilderExtensions
    {
        public static IApplicationBuilder UseAllAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<AllAuthenticationMiddleware>();
        }
    }
}