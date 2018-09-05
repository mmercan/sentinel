using Mercan.Common.Security;
using Microsoft.AspNetCore.Builder;

namespace Mercan.Common.Certificate
{
    /// <summary>
    /// Extension methods to register client certificate validation middleware
    /// </summary>
    public static class ClientCertificateValidation
    {
        /// <summary>
        /// Adds the client certificate validation middleware to the AspNet Core pipeline.
        /// Expects options/settings of type ValidateCertificateSettings to be specified in application configuration.
        /// The concrete type ClientCertificateMiddleware is used.
        /// </summary>       
        public static IApplicationBuilder UseClientCertificateValidationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClientCertificateValidationMiddleware>();
        }

        /// <summary>
        /// Adds the client certificate validation middleware to the AspNet Core pipeline. A custom certificate provider
        /// can be specified to provide the certificate against which the client certificate is validated.        ///  
        /// Expects options/settings of type ValidateCertificateSettings to be specified in application configuration.
        /// The concrete type ClientCertificateMiddleware is used.
        /// </summary>  
        public static IApplicationBuilder UseClientCertificateValidationMiddleware(this IApplicationBuilder builder,
            ICertificateProvider compareCertificateProvider)
        {
            return builder.UseMiddleware<ClientCertificateValidationMiddleware>(compareCertificateProvider);
        }
    }
}
