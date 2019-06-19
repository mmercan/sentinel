using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.HttpRequest.Models
{
    public class HttpClientOptions
    {
        /// <summary>
        /// Gets or sets the unique Id for the HttpClient
        /// For each unique Id, only a single HttpClient instance will be created to avoid socket exhaustion
        /// </summary>
        //public string Id { get; set; }

        /// <summary>
        /// Gets or sets the base address
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the default request header values
        /// </summary>
        public Dictionary<string, string> DefaultRequestHeaders { get; set; }

        /// <summary>
        /// Gets or sets the request content type
        /// </summary>
        public string RequestContentType { get; set; }

        /// <summary>
        /// Gets or sets the Base64 encoded client certificate
        /// </summary>
        public string ClientCertificateBase64 { get; set; }

        /// <summary>
        /// Gets or sets the client certificate's thumbprint
        /// </summary>
        public string CertificateThumbprint { get; set; }

        /// <summary>
        /// Get or sets the jwt token options.
        /// </summary>
        public JwtTokenOptions JwtTokenOptions { get; set; }
    }
}
