namespace Mercan.Common.Constants
{
    public static class HttpConstants
    {
        /// <summary>
        /// Media Type used in Content Type/Accepts headers
        /// </summary>
        public const string MediaTypeApplicationJson = "application/json";

        /// <summary>
        /// Media type used for Bupa APIs that return JSON. The API version is suffixed to this (e.g. 'application/vnd.bupa+jsonv=1.1')
        /// </summary>
        public const string BupaJsonMediaType = "application/vnd.bupa+json";
        /// <summary>
        /// Default version parameter name used with Bupa JSON media type. 
        /// </summary>
        public const string BupaJsonMediaTypeVersionParameterName = "version";

        /// <summary>
        /// Name of Request Correlation Header
        /// </summary>
        public const string RequestCorrelationHeaderName = "X-Correlation-Request";

        /// <summary>
        /// Name of Session Correlation Header
        /// </summary>
        public const string SessionCorrelationHeaderName = "X-Correlation-Session";

        /// <summary>
        /// Name of ARR Client Certificate Header
        /// </summary>
        public const string ARRClientCertificateHeaderName = "X-ARR-ClientCert";
    }
}
