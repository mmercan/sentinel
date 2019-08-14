using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.HttpRequest.Models
{
    public class JwtTokenOptions
    {
        /// <summary>
        /// Gets or sets the authorityUrl
        /// </summary>
        public string AuthorityUrl { get; set; }

        /// <summary>
        /// Gets or sets the user name to be authorized
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password of authorized user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the client id to be authorized.
        /// </summary>
        public string ClientId { get; set; }
    }
}
