namespace Mercan.Common.Constants
{
    public static class HttpConstants
    {
        /// <summary>
        /// Media Type used in Content Type/Accepts headers
        /// </summary>
        public const string MediaTypeApplicationJson = "application/json";

        /// <summary>
        /// </summary>
        public const string JsonMediaType = "application/vnd+json";
        /// <summary>
        /// </summary>
        public const string JsonMediaTypeVersionParameterName = "version";

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
