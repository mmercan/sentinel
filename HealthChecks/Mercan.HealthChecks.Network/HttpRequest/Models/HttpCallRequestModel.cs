using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.HttpRequest.Models
{
    public class HttpCallRequestModel
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string BearerToken { get; set; }
        public string Base64Certificate { get; set; }
        public string CertificatePassword { get; set; }
        public string SubscriptionKey { get; set; }
        public bool IsResponseJson { get; set; }
    }
}