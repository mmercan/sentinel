using Sentinel.UI.STS.Models;
using Sentinel.UI.STS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Sentinel.UI.STS.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IOptions<TokenSettings> tokenSettings;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger<TokenController> logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenRepository tokenRepo;

        public TokenController(IOptions<TokenSettings> tokenSettings,
           ITokenRepository tokenRepository,
           UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager,
           ILogger<TokenController> logger,
           RoleManager<IdentityRole> roleManager
        )
        {
            this.tokenSettings = tokenSettings;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.roleManager = roleManager;
            this.tokenRepo = tokenRepository;
        }


        /// <summary>
        /// Authenticate User
        /// </summary>
        [HttpPost("auth")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ApiExplorerSettings(GroupName = @"Token")]

        public async Task<IActionResult> Auth([FromBody]AuthRequest authUserRequest)
        {
            logger.LogInformation("Grant Type is " + authUserRequest.GrantType);

            if (!this.ModelState.IsValid)
            {
                return Json(new
                {
                    Code = "901",
                    Message = "ModelState isnot valid",
                    // Data = null
                });
            }



            if (authUserRequest.GrantType == "password")
            {
                return await Login(authUserRequest);
            }
            else if (authUserRequest.GrantType == "refresh_token")
            {
                return Json(new
                {
                    Code = "901",
                    Message = "null of parameters",
                    // Data = null
                });
                // return Json(DoRefreshToken(authUserRequest));
            }
            else
            {
                return Json(new
                {
                    Code = "904",
                    Message = "bad request",
                    // Data = null
                });
            }
        }


        private async Task<IActionResult> Login(AuthRequest authUserRequest)
        {
            var user = await userManager.FindByEmailAsync(authUserRequest.UserName);
            if (user != null)
            {
                var checkPwd = await signInManager.CheckPasswordSignInAsync(user, authUserRequest.Password, false);
                var roles = await userManager.GetRolesAsync(user);
                var roleString = JsonConvert.SerializeObject(roles);
                if (checkPwd.Succeeded)
                {
                    bool refreshTokenDone = true;
                    string refreshToken = null;
                    if (tokenSettings.Value.MultipleRefreshTokenEnabled)
                    {
                        refreshToken = Guid.NewGuid().ToString().Replace("-", "");
                        var tokenRepoModel = new TokenRepoModel
                        {
                            ClientId = authUserRequest.ClientId,
                            RefreshToken = refreshToken,
                            Id = Guid.NewGuid().ToString(),
                            IsStop = 0
                        };
                        refreshTokenDone = tokenRepo.AddToken(tokenRepoModel);
                    }

                    if (refreshTokenDone)
                    {
                        var response = await GetJwt(user, roles, authUserRequest.ClientId, refreshToken);
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Code = "909",
                            Message = "can not add token to database",
                        });
                    }

                }
            }


            return BadRequest(
             new
             {
                 Code = "902",
                 Message = "invalid user infomation",

             });
        }
        private async Task<string> GetJwt(IdentityUser user, IList<string> userRoles, string client_id, string refresh_token)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                            new Claim(ClaimTypes.Name,  user.UserName),
                        };
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }


            // var symmetricKeyAsBase64 = tokenSettings.Value.Secret;
            // var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            // var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Secret));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: tokenSettings.Value.Issuer,
                audience: tokenSettings.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: creds);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds,
                refresh_token = refresh_token,
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// Get Claim list All
        /// </summary>
        [Authorize]
        [HttpGet("claims", Name = "GetClaims")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public object Claims()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        /// <summary>
        /// Get Claim list local
        /// </summary>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = "local")]
        [HttpGet("claimsjwt", Name = "GetClaimsJwt")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public object ClaimsJwt()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }


        /// <summary>
        /// Get Claim list Azure
        /// </summary>
        [Authorize(AuthenticationSchemes = "azure")]
        [HttpGet("claimsazure")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public object ClaimsAzure()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        /// <summary>
        /// Validate User
        /// </summary>
        [Authorize(AuthenticationSchemes = "local")]
        [HttpPost("validateuser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<object> Validate([FromBody]ValidateTokenRequest tokenRequest)
        {
            logger.LogInformation("Email addess : " + tokenRequest.UserName);
            var user = await userManager.FindByEmailAsync(tokenRequest.UserName);
            bool exists = user != null;
            if (!exists) return BadRequest("The user was not found.");
            string tokenUsername = ValidateToken(tokenRequest.Token);

            if (tokenRequest.UserName.Equals(tokenUsername))
                return Ok();
            return BadRequest();
        }


        /// <summary>
        /// Get Principal from token
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Secret));
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);

                logger.LogDebug("GetPrincipal Returned : " + principal.Identity.Name);
                return principal;
            }
            catch (Exception e)
            {
                logger.LogError("Exception : " + e.Message);
                return null;
            }
        }


        private string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            logger.LogDebug("ValidateToken Returned : " + username);
            return username;
        }

    }
}
