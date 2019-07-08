using System;
using Microsoft.AspNetCore.Authentication;
using Mercan.Common.AspNetCore;
using Mercan.Common.AspNetCore.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AuthAppBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="AuthenticationMiddleware"/> to the specified <see cref="IApplicationBuilder"/>, which enables authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
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