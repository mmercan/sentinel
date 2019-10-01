using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;
using Mercan.HealthChecks.Network;
using Mercan.HealthChecks.Network.HttpRequest.Models;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;

namespace Mercan.HealthChecks.Network.Tests.Checks
{
    public class ModelsShould
    {

        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateaNetworkInstance()
        {
            var a1 = new NetworkHealthCheck("google.com:443");

            var a2 = new HttpCallRequestModel
            {
                Url = "",
                Method = "",
                BearerToken = "",
                Base64Certificate = "",
                CertificatePassword = "",
                SubscriptionKey = "",
                IsResponseJson = false
            };
            var a3 = new HttpCallRespondModel
            {
                RequestModel = a2,
                // StatusCode= HttpStatusCode.OK,
                Responsetext = "",
                ErrorMessage = "",
                ProcessMessages = "",
                // ResponseMessage= new HttpResponseMessage()
            };

            var a4 = new JwtTokenOptions
            {
                AuthorityUrl = "",
                UserName = "",
                Password = "",
                ClientId = ""
            };

            var a5 = new HttpClientOptions
            {
                BaseAddress = "",
                DefaultRequestHeaders = new Dictionary<string, string>(),
                RequestContentType = "",
                ClientCertificateBase64 = "",
                CertificateThumbprint = "",
                JwtTokenOptions = a4,
            };
        }

    }
}
