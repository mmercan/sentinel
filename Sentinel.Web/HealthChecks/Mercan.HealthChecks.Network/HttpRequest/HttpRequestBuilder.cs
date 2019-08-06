using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.HttpRequest
{
    public class HttpRequestBuilder
    {
        private HttpMethod method = null;
        private string requestUri = "";
        private HttpContent content = null;
        private string bearerToken = "";
        private string acceptHeader = "application/json";
        private TimeSpan timeout = new TimeSpan(0, 0, 15);
        private bool allowAutoRedirect = false;
        private string base64Certificate = null;
        private string certificatePassword = null;
        private string subscriptionKey = null;

        Dictionary<string, string> headers = null;

        ILogger logger = null;
        private string requestContentType;

        public HttpRequestBuilder()
        {
        }
        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }
        public HttpRequestBuilder AddLogger(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        public HttpRequestBuilder AddRequestContentType(string requestContentType)
        {
            this.requestContentType = requestContentType;
            return this;
        }

        public HttpRequestBuilder AddHeaders(Dictionary<string, string> headers)
        {
            this.headers = headers;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            this.bearerToken = bearerToken;
            return this;
        }


        public HttpRequestBuilder AddSubscriptionKey(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey;
            return this;
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public HttpRequestBuilder AddAllowAutoRedirect(bool allowAutoRedirect)
        {
            this.allowAutoRedirect = allowAutoRedirect;
            return this;
        }

        public HttpRequestBuilder AddClientCertificate(string base64Certificate)
        {
            this.base64Certificate = base64Certificate;
            return this;
        }
        public HttpRequestBuilder AddClientCertificate(string base64Certificate, string certificatePassword)
        {
            this.base64Certificate = base64Certificate;
            this.certificatePassword = certificatePassword;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            EnsureArguments();
            this.LogDebug("SendAsync called for " + requestUri, null);
            var request = new HttpRequestMessage
            {
                Method = this.method,
                RequestUri = new Uri(this.requestUri),
            };

            if (this.content != null) request.Content = this.content;
            if (!string.IsNullOrEmpty(this.bearerToken)) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.bearerToken);

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this.acceptHeader)) request.Headers.Add("Accept", "application/json;version=1.0");
            if (!string.IsNullOrEmpty(this.subscriptionKey))
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Headers.Add("Ocp-Apim-Trace", "true");
            }


            if (this.headers != null && this.headers.Count > 0)
            {
                foreach (var item in this.headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            if (!string.IsNullOrWhiteSpace(requestContentType))
            {
                //if (request.Content == null)
                //{
                //    request.Content =  new JsonContent("");
                //}
                //request.Content.Headers.ContentType = new MediaTypeHeaderValue(requestContentType);
            }

            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = this.allowAutoRedirect;
            if (!string.IsNullOrEmpty(base64Certificate))
            {
                X509Certificate2 cert = null;
                if (string.IsNullOrEmpty(certificatePassword))
                {
                    cert = new X509Certificate2(Convert.FromBase64String(base64Certificate));
                }
                else
                {
                    cert = new X509Certificate2(Convert.FromBase64String(base64Certificate), certificatePassword);
                }
                this.LogDebug("SendAsync " + requestUri + " Thumbprint added : " + cert.Thumbprint, null);
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.SslProtocols = SslProtocols.Tls12;
                handler.ClientCertificates.Add(cert);
            }

            var client = new System.Net.Http.HttpClient(handler);
            client.Timeout = this.timeout;
            //if (!string.IsNullOrWhiteSpace(requestContentType))
            //{
            //    client.DefaultRequestHeaders.Add("Content-Type", requestContentType);
            //}
            return await client.SendAsync(request);
        }
        private void LogDebug(string message, object[] args)
        {

            if (logger != null)
            {
                logger.LogDebug(message, args);
            }
        }

        private void EnsureArguments()
        {
            if (this.method == null)
                throw new ArgumentNullException("Method");

            if (string.IsNullOrEmpty(this.requestUri))
                throw new ArgumentNullException("Request Uri");
        }



    }
}
