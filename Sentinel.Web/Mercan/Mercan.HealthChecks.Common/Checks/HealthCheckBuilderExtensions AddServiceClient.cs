﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Mercan.HealthChecks.Common.Checks
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddApiIsAlive(this IHealthChecksBuilder builder, IConfiguration clientOptions, string isAliveUrl = "Health/IsAlive", TimeSpan? cacheDuration = null)
        {
            ApiServiceConfiguration config = new ApiServiceConfiguration();
            string sectionPath = "";
            if (clientOptions is Microsoft.Extensions.Configuration.ConfigurationSection)
            {
                sectionPath = (clientOptions as Microsoft.Extensions.Configuration.ConfigurationSection).Path;
            }
            clientOptions.Bind(config);
            string path = string.Format(config.BaseAddress + isAliveUrl);
            return builder.AddTypeActivatedCheck<ServiceClientBaseHealthCheck>($"ApiIsAlive {path} {sectionPath}", null, null, config, path);
            // builder.AddCheck($"ApiIsAlive {path} {sectionPath}", () =>
            // {
            //     try
            //     {
            //         ServiceClientBase service = new ServiceClientBase(config);
            //         var task = service.SendStringAsync(path, HttpMethod.Get);
            //         task.Wait();
            //         string response = task.Result;
            //         string description = path + " is succeesful with response : " + response;
            //         return HealthCheckResult.Healthy(description);
            //     }
            //     catch (Exception ex)
            //     {
            //         var Message = ex.InnerException?.InnerException?.Message;
            //         if (Message == null) { Message = ex.InnerException?.Message; }
            //         if (Message == null) { Message = ex.Message; }
            //         string description = Message;
            //         IReadOnlyDictionary<string, object> data = new Dictionary<string, object> { { path, " failed with exception " + Message }, { "BaseAddress", config?.BaseAddress } };
            //         return HealthCheckResult.Unhealthy(description, null, data);
            //     }
            // });
            // return builder;
        }

        // public static IHealthChecksBuilder AddApiIsAliveAndWell(this IHealthChecksBuilder builder, IConfiguration clientOptions, string isAliveAndWellUrl = "Health/IsAliveAndWell", TimeSpan? cacheDuration = null)
        // {
        //     ApiServiceConfiguration config = new ApiServiceConfiguration();
        //     clientOptions.Bind(config);
        //     string path = string.Format(config.BaseAddress + isAliveAndWellUrl);
        //     builder.AddCheck($"ApiIsAliveAndWell {path}", () =>
        //     {
        //         try
        //         {
        //             ServiceClientBase service = new ServiceClientBase(config);
        //             var task = service.SendAsync<HealthCheckResult>(path, HttpMethod.Get);
        //             task.Wait();
        //             var items = task.Result;
        //             string description = path + " is succeesful";
        //             return HealthCheckResult.Healthy(description);
        //         }
        //         catch (Exception ex)
        //         {
        //             var Message = ex.InnerException?.InnerException?.Message;
        //             if (Message == null) { Message = ex.InnerException?.Message; }
        //             if (Message == null) { Message = ex.Message; }
        //             string description = Message;
        //             IReadOnlyDictionary<string, object> data = new Dictionary<string, object> { { path, " failed with exception " + Message }, { "BaseAddress", config?.BaseAddress } };
        //             return HealthCheckResult.Unhealthy(description, null, data);
        //         }
        //     });
        //     return builder;
        // }
    }

    public class ApiServiceConfiguration
    {
        public ApiServiceConfiguration()
        {
            DefaultRequestHeaders = new Dictionary<string, string>();
            RequestHeaders = new Dictionary<string, string>();
        }
        /// <summary>
        /// Gets or sets the unique Id for the HttpClient
        /// For each unique Id, only a single HttpClient instance will be created to avoid socket exhaustion
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the base address
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the default request header values
        /// </summary>
        public Dictionary<string, string> DefaultRequestHeaders { get; set; }

        public Dictionary<string, string> RequestHeaders { get; set; }

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
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
    public class ServiceClientBaseHealthCheck : IHealthCheck
    {
        private readonly ApiServiceConfiguration _options;
        private static HttpClient _httpClient;
        protected HttpClient Client { get => _httpClient; private set => _httpClient = value; }
        ILogger<ServiceClientBaseHealthCheck> logger;
        string path;
        public ServiceClientBaseHealthCheck(ILogger<ServiceClientBaseHealthCheck> logger, ApiServiceConfiguration options, string path)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            this.path = path;
            this.logger = logger;
            InitClient();

            logger.LogCritical("ServiceClientBaseHealthCheck Init Completed");
        }

        private void InitClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            // add certificate to HttpClient
            X509Certificate2 certificate = CreateClientCertificate();
            if (certificate != null)
            {
                handler.ClientCertificates.Add(certificate);
            }

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_options.BaseAddress)
            };

            string token = GetToken();
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> header in _options.DefaultRequestHeaders)
            {
                if (!string.IsNullOrEmpty(header.Key))
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        public string GetToken()
        {
            string token = null;
            if (!string.IsNullOrEmpty(_options.ClientSecret) && !string.IsNullOrEmpty(_options.ClientId))
            {
                string clientId = _options.ClientId;
                string secret = _options.ClientSecret;
                string adId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8";
                string url = $"https://login.microsoftonline.com/{adId}/oauth2/token?resource={clientId}";
                var httpClient = new HttpClient();

                var dict = new Dictionary<string, string>();
                dict.Add("grant_type", "client_credentials");
                dict.Add("client_id", clientId);
                dict.Add("client_secret", secret);

                var httpContent = new FormUrlEncodedContent(dict);
                httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                var clientTask = httpClient.PostAsync(url, httpContent);
                clientTask.Wait();
                var ContentTask = clientTask.Result.Content.ReadAsStringAsync();
                ContentTask.Wait();

                JObject o = JObject.Parse(ContentTask.Result);
                token = o["access_token"].ToString();
            }
            return token;
        }

        private X509Certificate2 CreateClientCertificate()
        {
            X509Certificate2 certificate = null;
            if (!string.IsNullOrEmpty(_options.ClientCertificateBase64))
            {
                byte[] rawData = Convert.FromBase64String(_options.ClientCertificateBase64);
                certificate = new X509Certificate2(rawData);
            }
            else if (!string.IsNullOrEmpty(_options.CertificateThumbprint))
            {
                certificate = FindCertificateByThumbprint(_options.CertificateThumbprint);
            }
            return certificate;
        }
        public HttpContent CreateContent<T>(T content)
        {
            string serialisedContent = JsonConvert.SerializeObject(content);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(serialisedContent);

            ByteArrayContent httpContent = new ByteArrayContent(buffer);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(_options.RequestContentType);
            return httpContent;
        }
        private X509Certificate2 FindCertificateByThumbprint(string findValue)
        {
            X509Certificate2 certificate = null;
            if (!string.IsNullOrEmpty(findValue))
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    // Don't validate certs, since the test root isn't installed.
                    X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint, findValue, false);
                    if (col.Count > 0)
                    {
                        certificate = col[0];
                    }
                }
                finally
                {
                    store.Close();
                }
            }
            return certificate;
        }
        public async Task<TResponse> SendAsync<TResponse>(string path, HttpMethod httpMethod)
        {
            return await SendAsync<object, TResponse>(path, httpMethod, null);
        }
        protected async Task<TResponse> SendAsync<TContent, TResponse>(string path, HttpMethod httpMethod, TContent content)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string responseText = await SendAsync(path, httpMethod, content);
            TResponse responseContent = (string.IsNullOrEmpty(responseText)) ? default(TResponse) : JsonConvert.DeserializeObject<TResponse>(responseText);

            return responseContent;
        }
        public async Task<string> SendStringAsync(string path, HttpMethod httpMethod)
        {
            return await SendStringAsync<object>(path, httpMethod, null);
        }
        protected async Task<string> SendStringAsync<TContent>(string path, HttpMethod httpMethod, TContent content)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string responseText = await SendAsync(path, httpMethod, content);

            return responseText;
        }
        public async Task<string> SendAsync<TContent>(string path, HttpMethod httpMethod, TContent content)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            // var client_ = new System.Net.Http.HttpClient();
            // using (var request_ = new System.Net.Http.HttpRequestMessage())
            // {
            //     request_.Method = new System.Net.Http.HttpMethod("GET");
            //     request_.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //     request_.RequestUri = new System.Uri(path, System.UriKind.RelativeOrAbsolute);

            //     var response_ = client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
            //     var responseText_ = await response_.Content.ReadAsStringAsync();
            //     logger.LogCritical("response from the addHere " + responseText_);
            // }


            string responseText = string.Empty;
            logger.LogCritical("Request is Ready to Send to " + path);
            using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, path))
            {
                if (content != null)
                {
                    HttpContent httpContent = CreateContent(content);
                    request.Content = httpContent;
                }

                HttpResponseMessage response;

                try
                {
                    response = await Client.SendAsync(request);
                    logger.LogCritical("response is received " + path);
                    responseText = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    var Message = ex.InnerException?.InnerException?.Message;
                    if (Message == null) { Message = ex.InnerException?.Message; }
                    if (Message == null) { Message = ex.Message; }
                    throw new HttpRequestException($"Request {httpMethod} {path} failed. with Exception : {Message}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new HttpRequestException($"Request {httpMethod} {path} failed. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}");
                    }
                    else
                    {
                        throw new HttpRequestException($"Request {httpMethod} {path} failed. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}");
                    }
                }
            }
            return responseText.Trim();
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var task = SendStringAsync(path, HttpMethod.Get);
                task.Wait();
                string response = task.Result;
                string description = path + " is succeesful with response : " + response;
                return HealthCheckResult.Healthy(description);
            }
            catch (Exception ex)
            {
                var Message = ex.InnerException?.InnerException?.Message;
                if (Message == null) { Message = ex.InnerException?.Message; }
                if (Message == null) { Message = ex.Message; }
                string description = Message;
                IReadOnlyDictionary<string, object> data = new Dictionary<string, object> { { path, " failed with exception " + Message }, { "BaseAddress", _options?.BaseAddress } };
                return HealthCheckResult.Unhealthy(description, null, data);
            }
        }
    }

}
