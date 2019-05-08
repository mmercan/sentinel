using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.HttpRequest.Models
{
    public class HttpCallRespondModel
    {

        public HttpCallRequestModel RequestModel { get; set; }
        public HttpStatusCode StatusCode { get; internal set; }
        public string Responsetext { get; set; }
        public string ErrorMessage { get; set; }
        public string ProcessMessages { get; set; }
        public HttpResponseMessage ResponseMessage { get; internal set; }

    }
}