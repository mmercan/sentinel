using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sentinel.UI.STS.Models
{
    public class RequestResult
    {
        public RequestState State { get; set; }
        public string Msg { get; set; }
        public Object Data { get; set; }
    }

    public enum RequestState
    {
        Failed = -1,
        NotAuth = 0,
        Success = 1
    }

    public class AuthRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class TokenSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool MultipleRefreshTokenEnabled { get; set; }
    }

    public class TokenRepoModel
    {
        [Key]
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string RefreshToken { get; set; }
        public int IsStop { get; set; }
    }

    public class ValidateTokenRequest
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
