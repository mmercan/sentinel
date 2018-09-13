Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip($zipfile, $outdir) {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $archive = [System.IO.Compression.ZipFile]::OpenRead($zipfile)
    foreach ($entry in $archive.Entries) {
        $entryTargetFilePath = [System.IO.Path]::Combine($outdir, $entry.FullName)
        $entryDir = [System.IO.Path]::GetDirectoryName($entryTargetFilePath)
        
        #Ensure the directory of the archive entry exists
        if (!(Test-Path $entryDir )) {
            New-Item -ItemType Directory -Path $entryDir | Out-Null 
        }
        #If the entry is not a directory entry, then extract entry
        if (!$entryTargetFilePath.EndsWith("/")) {
            [System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $entryTargetFilePath, $true);
        }
    }
}


# function Add-Folder {
#     param($folder)
#     $global:folder = $folder
#     # $folder = "AWWeb.Web.Sts"
#     $scriptpath = $MyInvocation.ScriptName # $MyInvocation.MyCommand.Path 
#     $dir = Split-Path $scriptpath
#     $appFolder = Join-Path -Path $dir -ChildPath ..\..\..\prototypes\identity\$folder
#     $appRootFolder = Join-Path -Path $dir -ChildPath ..\..\..\prototypes\identity
#     # $appFolder

# }

#$folder = "AWWeb.Web.Sts"
#$scriptpath = $MyInvocation.MyCommand.Path 
#$dir = Split-Path $scriptpath

#$appFolder = Join-Path -Path $dir -ChildPath ..\..\prototypes\identity\$folder
#$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\prototypes\identity
#$appFolder

function new-dotnet-Individual {
    dotnet new mvc --auth  Individual # --output $folder
    
    $fileName = "./Properties/launchSettings.json"
    $launchSettings = Get-Content $fileName  -raw
    $launchSettings = $launchSettings.replace('"applicationUrl": "https://localhost:5001;http://localhost:5000"', '"applicationUrl": "http://localhost:5000"')
    $launchSettings | set-content  $fileName


    dotnet add package "WebPush-netcore"

    dotnet add package "Swashbuckle.AspNetCore"
    dotnet add package "Microsoft.AspNetCore.Mvc.Versioning"
    dotnet add package "Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer"

    dotnet add package "Microsoft.AspNetCore.Identity"  -v 2.1.2
    dotnet add package "Microsoft.AspNetCore.Identity.UI"  -v 2.1.2
    dotnet add package "Microsoft.AspNetCore.Identity.EntityFrameworkCore"  -v 2.1.2

    dotnet add package "AutoMapper.Extensions.Microsoft.DependencyInjection"
}


function new-dotnet ([string]$port = 5000) {
    dotnet new mvc # --output $folder
    $loc = '"applicationUrl": "http://localhost:' + $port + '"'


    $fileName = "./Properties/launchSettings.json"
    $launchSettings = Get-Content $fileName  -raw
    $launchSettings = $launchSettings.replace('"applicationUrl": "https://localhost:5001;http://localhost:5000"', $loc)
    $launchSettings | set-content  $fileName

    dotnet add package "WebPush-netcore"
    dotnet add package "Polly"
    dotnet add package "Swashbuckle.AspNetCore"
    dotnet add package "Microsoft.AspNetCore.Mvc.Versioning"
    dotnet add package "Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer"

    dotnet add package "Microsoft.AspNetCore.Identity" -v 2.1.2
    dotnet add package "Microsoft.AspNetCore.Identity.UI"  -v 2.1.2
    dotnet add package "Microsoft.AspNetCore.Identity.EntityFrameworkCore"  -v 2.1.2

    dotnet add package "AutoMapper.Extensions.Microsoft.DependencyInjection"
}

function Add-PushNotificationController {
    $PushNotificationController = @'
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using __projectname__.Models;
using WebPush;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace __projectname__.Controllers
{
    [Route("api/[controller]")]
    public class PushNotificationController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PushNotificationController> _logger;

        public PushNotificationController(IConfiguration configuration, ILogger<PushNotificationController> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //TODO: Implement Realistic Implementation
            // return Content("Blahhh");
            // return Ok("Blah");
            return Content("Blah");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "azure")]
        public IActionResult Post([FromBody] PushNotificationModel model, [FromHeader]string Email)
        {
            _logger.LogDebug("Post Called");
            var iden = this.User.Identity;
            var email = this.User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            //TODO: Implement Realistic Implementation
            model.Email = Email;
            var payload = JsonConvert.SerializeObject(
              new
              {
                  Email = Email,
                  Message = "Welcome",
                  Link = "null"
              }
            );
            Debug.WriteLine(payload);

            NofityUser(model.Endpoint, model.Keys.P256dh, model.Keys.Auth, payload);
            return Created("", null);
        }


        private void NofityUser(string endpoint, string p256dh, string auth, string payload)
        {
            var vapidDetails = new VapidDetails(@"mailto:mmercan@outlook.com"
                , "BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA"
                , "r2HJzuoJiFD0uMDoQcKMQCGo8M80wag8kCoTMFf3S34"
              );
            var client = new WebPushClient();
            var subs = new PushSubscription(endpoint, p256dh, auth);

            var task = client.SendNotificationAsync(subs, payload, vapidDetails);
            task.Wait();
            var status = task.Status;
        }

        [HttpPost("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult JustforUsers([FromBody] PushNotificationModel model, [FromHeader]string Email)
        {
            return Content("Ok");
        }
    }
}
'@

    $PushNotificationController = $PushNotificationController.replace("__projectname__", $folder)
    $PushNotificationController | set-content  ".\Controllers\PushNotificationController.cs"

    $PushNotificationModel = @'
using System;

namespace __projectname__.Controllers
{
    public class PushNotificationModel
    {
        public string Email { get; set; }
        public string Endpoint { get; set; }
        public string ExpirationTime { get; set; }
        public KeyReference Keys { get; set; }
    }

    public class KeyReference
    {
        public string P256dh { get; set; }
        public string Auth { get; set; }

    }
}

'@
    $PushNotificationModel = $PushNotificationModel.replace("__projectname__", $folder)
    $PushNotificationModel | set-content  ".\Models\PushNotificationModel.cs"
}
function Add-TokenController {

    new-item -type directory -path $appFolder\DbContexts -Force
    new-item -type directory -path $appFolder\Repositories -Force
    new-item -type directory -path $appFolder\Models -Force

    $TokenController = @'
    using __projectname__.Models;
    using __projectname__.Repositories;
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

    namespace __projectname__.Controllers
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

        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody]AuthRequest authUserRequest)
        {
            logger.LogInformation("Grant Type is " + authUserRequest.GrantType);
            if (authUserRequest == null)
            {
                return Json(new
                {
                    Code = "901",
                    Message = "null of parameters",
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


        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }



        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = "local")]
        [HttpGet("claimsjwt")]
        public object ClaimsJwt()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        [Authorize(AuthenticationSchemes = "azure")]
        [HttpGet("claimsazure")]
        public object ClaimsAzure()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }


        [Authorize(AuthenticationSchemes = "local")]
        [HttpPost("validateuser")]
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

        [HttpGet()]
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
'@
    $TokenController = $TokenController.replace("__projectname__", $folder)
    $TokenController | set-content  ".\Controllers\TokenController.cs"

    $TokenDbContext = @'
    using Microsoft.EntityFrameworkCore;
    using System.IO;
    using __projectname__.Models;
    namespace __projectname__.DbContexts
    {
        public class TokenDbContext : DbContext
        {
            public DbSet<TokenRepoModel> Tokens { get; set; }
            public TokenDbContext(DbContextOptions<TokenDbContext> options): base(options)
            { }
            //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //{
            //    var connStr = Path.Combine(Directory.GetCurrentDirectory(), "demo.db");
            //    optionsBuilder.UseSqlite($"Data Source={connStr}");
            //}
        }
    }
'@
    $TokenDbContext = $TokenDbContext.replace("__projectname__", $folder)
    $TokenDbContext | set-content  ".\DbContexts\TokenDbContext.cs"

    $TokenRepository = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using __projectname__.DbContexts;
    using __projectname__.Models;

    namespace __projectname__.Repositories
    {
        public class TokenRepository : ITokenRepository
        {
            private TokenDbContext dbContext;
            public TokenRepository(TokenDbContext dbContext)
            {
                this.dbContext = dbContext;
            }
            public bool AddToken(TokenRepoModel token)
            {
                dbContext.Tokens.Add(token);
                return dbContext.SaveChanges() > 0;
            }
            public bool ExpireToken(TokenRepoModel token)
            {
                dbContext.Tokens.Update(token);
                    return dbContext.SaveChanges() > 0;
            }
            public TokenRepoModel GetToken(string refresh_token, string client_id)
            {
                    return dbContext.Tokens.FirstOrDefault(x => x.ClientId == client_id && x.RefreshToken == refresh_token);
            }
        }
    }
"@
    $TokenRepository = $TokenRepository.replace("__projectname__", $folder)
    $TokenRepository | set-content  ".\Repositories\TokenRepository.cs"

    $ITokenRepository = @"
using __projectname__.Models;
namespace __projectname__.Repositories
{
    public interface ITokenRepository
    {
        bool AddToken(TokenRepoModel token);
        bool ExpireToken(TokenRepoModel token);
        TokenRepoModel GetToken(string refresh_token, string client_id);
    }
}
"@
    $ITokenRepository = $ITokenRepository.replace("__projectname__", $folder)
    $ITokenRepository | set-content  ".\Repositories\ITokenRepository.cs"

    $RequestResult = @'
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace __projectname__.Models
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
'@
    $RequestResult = $RequestResult.replace("__projectname__", $folder)
    $RequestResult | set-content  ".\Models\RequestResult.cs"

    $jobj = Get-Content '.\appsettings.json' -raw | ConvertFrom-Json
    $tokens = @"
    {
        "Secret": "aHR0cDovL3d3dy5tbWVyY2FuLmNvbQ==",
        "Issuer": "http://www.mmercan.com",
        "Audience": "Matt Mercan",
        "MultipleRefreshTokenEnabled": true
    }
"@
    $AzureAd = @"
{
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "mrtmrcn.onmicrosoft.com",
    "TenantId": "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
    "ClientId": "67d009b1-97fe-4963-84ff-3590b06df0da",
    "CallbackPath": "/signin-oidc"
  }

"@
    $jobj| add-member -Name "Tokens" -value (Convertfrom-Json $tokens) -MemberType NoteProperty
    $jobj| add-member -Name "AzureAd" -value (Convertfrom-Json $AzureAd) -MemberType NoteProperty
    $json = $jobj| ConvertTo-Json -Depth 5
    $json | set-content  '.\appsettings.json'
}
function Add-Sts-startupcs {

    $fileName = "$appFolder\Startup.cs"
    $startupobj = (Get-Content $fileName) |
        Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "using System;") {
            'using Microsoft.IdentityModel.Tokens;'
            'using System.Security.Claims;'
            'using Microsoft.AspNetCore.Authentication.JwtBearer;'
            'using Microsoft.AspNetCore.Authorization;'
            'using __projectname__.Models;'
            'using __projectname__.DbContexts;'
            'using __projectname__.Repositories;'
        }
        if ($_ -match "services.AddDefaultIdentity<IdentityUser>()") {
            '            .AddRoles<IdentityRole>()'
        }
        if ($_ -match "Configuration.GetConnectionString") {
            '           .AddDbContext<TokenDbContext>(options =>options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));'
        }
        if ($_ -match ".AddEntityFrameworkStores<ApplicationDbContext>()") {
            #Add Lines after the selected pattern
            '           .AddDefaultTokenProviders();'
            ''
            '           services.Configure<TokenSettings>(Configuration.GetSection("Tokens"));'
            '           services.AddScoped<ITokenRepository, TokenRepository>();'
            '           services.AddAuthentication(sharedOptions =>'
            '           {'
            '               sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;'
            '               sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;'
            '           })'
            '           .AddJwtBearer("azure", cfg =>'
            '           {'
            '               cfg.RequireHttpsMetadata = false;'
            '               cfg.SaveToken = true;'
            '               cfg.Authority = Configuration["AzureAd:Instance"] + "/" + Configuration["AzureAD:TenantId"];'
            '               cfg.Audience = Configuration["AzureAd:ClientId"];'
            '           })'
            '           .AddJwtBearer("local", cfg =>'
            '           {'
            '               cfg.RequireHttpsMetadata = false;'
            '               cfg.SaveToken = true;'
            '               cfg.TokenValidationParameters = new TokenValidationParameters()'
            '               {'
            '                   ValidIssuer = Configuration["Tokens:Issuer"],'
            '                   ValidAudience = Configuration["Tokens:Audience"],'
            '                   IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:Secret"])),'
            '                   RoleClaimType = ClaimTypes.Role'
            '               };'
            '           });'
            ''
            '           //use both jwt schemas interchangeably  https://stackoverflow.com/questions/49694383/use-multiple-jwt-bearer-authentication'
            '           services.AddAuthorization(options =>'
            '           {'
            '               options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().AddAuthenticationSchemes("azure", "local").Build();'
            '           });'
            ''
        }
        # if ($_ -match "UseStaticFiles") {
        #     '           using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())'
        #     '           {'
        #     '               var context = serviceScope.ServiceProvider.GetRequiredService<TokenDbContext>();'
        #     '               context.Database.EnsureCreated();'
        #     '               var context2 = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();'
        #     '               context2.Database.Migrate();'
        #     '           }'
        # }

    }

    $startupobj = $startupobj.replace(".AddEntityFrameworkStores<ApplicationDbContext>();", ".AddEntityFrameworkStores<ApplicationDbContext>()")
    $startupobj = $startupobj.replace(' Configuration.GetConnectionString("DefaultConnection")));', ' Configuration.GetConnectionString("DefaultConnection")))')
    $startupobj = $startupobj.replace("__projectname__", $folder)
    $startupobj | set-content  $fileName
}
function Add-Api-ConfigureJwtAuthService-startupcs {

    $fileName = "$appFolder\Startup.cs"
    $startupobj = (Get-Content $fileName) |
        Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "using System;") {
            'using Microsoft.IdentityModel.Tokens;'
            'using System.Security.Claims;'
            'using Microsoft.AspNetCore.Authentication.JwtBearer;'
            'using System.Text;'
            'using Microsoft.AspNetCore.Authorization;'
            'using AutoMapper;'
            '// using __projectname__.Models;'
            '// using __projectname__.DbContexts;'
            '// using __projectname__.Repositories;'
        }
        if ($_ -match "public IConfiguration Configuration") {
            ''
            '        public void ConfigureJwtAuthService(IServiceCollection services)'
            '        {'
            '             var audienceConfig = Configuration.GetSection("Tokens");'
            '             var symmetricKeyAsBase64 = audienceConfig["Secret"];'
            '             var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);'
            '             var signingKey = new SymmetricSecurityKey(keyByteArray);'
            ''
            '            var tokenValidationParameters = new TokenValidationParameters'
            '            {'
            '                // The signing key must match!'
            '                ValidateIssuerSigningKey = true,'
            '                IssuerSigningKey = signingKey,'
            ''
            '                // Validate the JWT Issuer (iss) claim'
            '                ValidateIssuer = true,'
            '                ValidIssuer = audienceConfig["Issuer"],'
            ''
            '                // Validate the JWT Audience (aud) claim'
            '                ValidateAudience = true,'
            '                ValidAudience = audienceConfig["Audience"],'
            ''
            '                // Validate the token expiry'
            '                ValidateLifetime = true,'
            ''
            '                ClockSkew = TimeSpan.Zero'
            '            };'
            ''
            '            services.AddAuthentication(options =>'
            '            {'
            '                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;'
            '                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;'
            '            })'
            '          .AddJwtBearer("azure", cfg =>'
            '          {'
            '              cfg.RequireHttpsMetadata = false;'
            '              cfg.SaveToken = true;'
            '              cfg.Authority = Configuration["AzureAd:Instance"] + "/" + Configuration["AzureAD:TenantId"];'
            '              cfg.Audience = Configuration["AzureAd:ClientId"];'
            '          })'
            '            .AddJwtBearer("sts",cfg =>'
            '            {'
            '                cfg.TokenValidationParameters = tokenValidationParameters;'
            '            });'
            '           //use both jwt schemas interchangeably  https://stackoverflow.com/questions/49694383/use-multiple-jwt-bearer-authentication'
            '           services.AddAuthorization(options =>'
            '           {'
            '               options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().AddAuthenticationSchemes("azure", "sts").Build();'
            '           });'

            '        }'
        }
        if ($_ -match "services.AddMvcCore()") {
            '       services.AddAuthentication();'
            '       // .AddAzureAD(options => Configuration.Bind("AzureAd", options));'
            '       ConfigureJwtAuthService(services);'
            ' '
            '       services.AddAutoMapper();'
            ' '
            '       services.AddMvc(options =>'
            '       {'
            '             // var policy = new AuthorizationPolicyBuilder()'
            '             //     .RequireAuthenticatedUser()'
            '             //     .Build();'
            '             // options.Filters.Add(new AuthorizeFilter(policy));'
            '       })'
            '       .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);'
        }
        if ($_ -match "app.UseCookiePolicy()") {
            ''
            'app.UseAuthentication();'
            ''

        }
        if ($_ -match ".AddEntityFrameworkStores<ApplicationDbContext>()") {
            #Add Lines after the selected pattern
            '           .AddDefaultTokenProviders();'
            ''
            '           services.Configure<TokenSettings>(Configuration.GetSection("Tokens"));'
            '           services.AddScoped<ITokenRepository, TokenRepository>();'
            '           services.AddAuthentication(sharedOptions =>'
            '           {'
            '               sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;'
            '               sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;'
            '           })'
            '           .AddJwtBearer("azure", cfg =>'
            '           {'
            '               cfg.RequireHttpsMetadata = false;'
            '               cfg.SaveToken = true;'
            '               cfg.Authority = Configuration["AzureAd:Instance"] + "/" + Configuration["AzureAD:TenantId"];'
            '               cfg.Audience = Configuration["AzureAd:ClientId"];'
            '           })'
            '           .AddJwtBearer("local", cfg =>'
            '           {'
            '               cfg.RequireHttpsMetadata = false;'
            '               cfg.SaveToken = true;'
            '               cfg.TokenValidationParameters = new TokenValidationParameters()'
            '               {'
            '                   ValidIssuer = Configuration["Tokens:Issuer"],'
            '                   ValidAudience = Configuration["Tokens:Audience"],'
            '                   IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:Secret"])),'
            '                   RoleClaimType = ClaimTypes.Role'
            '               };'
            '           });'
            ''
            '           //use both jwt schemas interchangeably  https://stackoverflow.com/questions/49694383/use-multiple-jwt-bearer-authentication'
            '            services.AddAuthorization(options =>{options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().AddAuthenticationSchemes("azure", "Custom").Build();});'
            ''
        }
    }

    $startupobj = $startupobj.replace(" services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);", " ")
    $startupobj = $startupobj.replace("__projectname__", $folder)
    $startupobj | set-content  $fileName

    $jobj = Get-Content '.\appsettings.json' -raw | ConvertFrom-Json
    $tokens = @"
    {
        "Secret": "aHR0cDovL3d3dy5tbWVyY2FuLmNvbQ==",
        "Issuer": "http://www.mmercan.com",
        "Audience": "Matt Mercan",
        "MultipleRefreshTokenEnabled": true
    }
"@
    $AzureAd = @"
{
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "mrtmrcn.onmicrosoft.com",
    "TenantId": "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
    "ClientId": "67d009b1-97fe-4963-84ff-3590b06df0da",
    "CallbackPath": "/signin-oidc"
  }

"@
    $jobj| add-member -Name "Tokens" -value (Convertfrom-Json $tokens) -MemberType NoteProperty
    $jobj| add-member -Name "AzureAd" -value (Convertfrom-Json $AzureAd) -MemberType NoteProperty
    $json = $jobj| ConvertTo-Json -Depth 5
    $json | set-content  '.\appsettings.json'

}
function Add-cors-swagger-startupcs {

    $fileName = "$appFolder\Startup.cs"
    $startupobj = (Get-Content $fileName) |
        Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "using System;") {
            'using Swashbuckle.AspNetCore.Swagger;'
            'using Microsoft.AspNetCore.Mvc.ApiExplorer;'
            'using Microsoft.Extensions.Logging;'
            'using Microsoft.AspNetCore.Mvc.Versioning;'
        }
        if ($_ -match "services.AddMvc()") {
            '            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>'
            '            {'
            '                builder.AllowAnyOrigin()'
            '                .AllowAnyMethod()'
            '                .AllowAnyHeader()'
            '                .SetIsOriginAllowedToAllowWildcardSubdomains()'
            '                .AllowCredentials();'
            '            }));'
            ''
            '            services.AddApiVersioning(o =>'
            '            {'
            '                o.AssumeDefaultVersionWhenUnspecified = true;'
            '                o.DefaultApiVersion = new ApiVersion(1, 0);'
            '                o.ApiVersionReader = new HeaderApiVersionReader("api-version");'
            '            });'
            ''
            '            services.AddSwaggerGen(options =>'
            '            {'
            '                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();'
            '                foreach (var description in provider.ApiVersionDescriptions)'
            '                {'
            '                    options.SwaggerDoc('
            '                    description.GroupName,'
            '                    new Info()'
            '                    {'
            '                        Title = $"__projectname__ API {description.ApiVersion}",'
            '                        Version = description.ApiVersion.ToString()'
            '                    });'
            '                }'
            '            });'
        }
        if ($_ -match "UseStaticFiles") {
            '            // move  UseDefaultFiles to first line'
            '            // app.UseFileServer();'
            '            app.UseDefaultFiles();'
            '            app.UseSwagger();'
            '            app.UseSwaggerUI(options =>'
            '                {'
            '                    foreach (var description in provider.ApiVersionDescriptions)'
            '                    {'
            '                        options.SwaggerEndpoint('
            '                            $"/swagger/{description.GroupName}/swagger.json",'
            '                            description.GroupName.ToUpperInvariant());'
            '                    }'
            '                });'
            '             app.UseCors("MyPolicy");'
        }

    }
    $startupobj = $startupobj.replace("__projectname__", $folder)
    $startupobj = $startupobj.replace("services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);", "services.AddMvcCore().AddVersionedApiExplorer( o => o.GroupNameFormat = `"'v'VVV`" ); `n services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);")
    $startupobj = $startupobj.replace("IApplicationBuilder app, IHostingEnvironment env", "IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IApiVersionDescriptionProvider provider")
    $startupobj | set-content  $fileName
}
function Add-SignalR {
    dotnet add package Microsoft.AspNetCore.SignalR -v 1.0.0

    $charcs = @'
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using System.Linq;
    using Microsoft.AspNetCore.SignalR;

    namespace __projectname__.Hubs
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class ChatHub : Hub
        {
            public Task Send(string message)
            {
                var user = Context.User;
                return Clients.All.SendAsync("Send", message);
            }

            public override async Task OnConnectedAsync()
            {
            // await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());
            // var iden = this.Context.User.Identity;
            await base.OnConnectedAsync();
            }
            public override Task OnDisconnectedAsync(Exception exception)
            {
            return base.OnDisconnectedAsync(exception);
            }
        }

        public static class extesions
        {
            public static void UseSignalRJwtAuthentication(this IApplicationBuilder app)
            {
                app.Use(async (context, next) =>
                {
                    if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
                    {
                        if (context.Request.QueryString.HasValue)
                        {
                            var token = context.Request.QueryString.Value.Split('&').SingleOrDefault(x => x.Contains("authorization"))?.Split('=')[1];
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
                            }
                        }
                    }
                    await next.Invoke();
                });
            }
        }
    }
'@
    $charcs = $charcs.replace("__projectname__", $folder)

    # new-item -type directory -path $appFolder\Middlewares -Force
    mkdir Hubs
    $charcs | set-content  ".\Hubs\ChatHub.cs"
    $fileName = "$appFolder\Startup.cs"
    (Get-Content $fileName) |
        Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "AddMvc") {
            '           services.AddSignalR();'
        }
        if ($_ -match "using System;") {
            'using ' + $folder + '.Hubs;'
        }
        if ($_ -match "UseAuthentication") {
            '            app.UseSignalR(routes =>'
            '            {'
            '                routes.MapHub<ChatHub>("/hub/chat");'
            '            });'
        }
        if ($_ -match "UseStaticFiles") {
            '            app.UseSignalRJwtAuthentication();'
        }
    } | Set-Content $fileName
}
function Add-Logger {

    new-item -type directory -path $appFolder\Middlewares -Force

    dotnet add package "Serilog.AspNetCore"
    dotnet add package "Serilog.Sinks.Console"
    dotnet add package "Serilog.Sinks.File"
    #  dotnet add package "Serilog.Sinks.LogstashHttp"
    dotnet add package "Serilog.Settings.Configuration"
    dotnet add package "Serilog.Sinks.Loggly"
    dotnet add package "Serilog.Sinks.ElasticSearch"

    $fileName = "$appFolder\Startup.cs"
    (Get-Content $fileName) |
        Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "using System;") {
            'using Serilog;'
            'using Serilog.Events;'
        }
        if ($_ -match "UseStaticFiles") {
            '            var logger = new LoggerConfiguration()'
            '            .ReadFrom.Configuration(Configuration)'
            '            .Enrich.FromLogContext()'
            '            .Enrich.WithProperty("Enviroment", env.EnvironmentName)'
            '            .Enrich.WithProperty("ApplicationName", "Api App")'
            '            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)'
            '            .WriteTo.Console()'
            '            .WriteTo.File("Logs/logs.txt");'
            '            //.WriteTo.Elasticsearch()'
            ''
            '            logger.WriteTo.Console();'
            '            loggerFactory.AddSerilog();'
            '            Log.Logger = logger.CreateLogger();'
            '            app.UseExceptionLogger();'
        }
    } | Set-Content $fileName

    $jobj = Get-Content '.\appsettings.json' -raw | ConvertFrom-Json
    $Serilog = @"
    {
        "Using": ["Serilog.Sinks.File"],
        "MinimumLevel": "Information",
        "WriteTo": [{
            "Name": "File",
            "Args": {
                "path": "Logs\\logs.txt",
                "rollingInterval": "Day",
                "outputTemplate": "{Timestamp:dd-MMM-yyyy HH:mm:ss.fff zzz} [{Level:u3}] [{Enviroment}] {Message:lj}{NewLine}{Exception}{ActionName} {RequestPath}{NewLine}{Exception}{NewLine}"
            }
        }]
    }
"@

    $jobj| add-member -Name "Serilog" -value (Convertfrom-Json $Serilog) -MemberType NoteProperty
    $json = $jobj| ConvertTo-Json -Depth 5
    $json | set-content  '.\appsettings.json'

    $ExceptionLoggerMiddleware = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace __projectname__
{
    public class ExceptionLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DeveloperExceptionPageOptions _options;
        private readonly ILogger _logger;

        public ExceptionLoggerMiddleware(
            RequestDelegate next,
            IOptions<ExceptionLoggerOptions> options,
            ILoggerFactory loggerFactory
            )
        {

            _logger = loggerFactory.CreateLogger<ExceptionLoggerMiddleware>();
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                var unhandledException = LoggerMessage.Define(LogLevel.Error, new EventId(1, "UnhandledException"), "An unhandled exception has occurred while executing the request.");
                unhandledException(_logger, ex);


                if (httpContext.Response.HasStarted)
                {
                }
                throw;
            }
        }
    }
    public class ExceptionLoggerOptions
    {
        public string Name { get; set; }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggerMiddleware>();
        }
    }
}

"@
    $ExceptionLoggerMiddleware = $ExceptionLoggerMiddleware.replace("__projectname__", $folder)
    $ExceptionLoggerMiddleware | set-content  ".\Middlewares\ExceptionLoggerMiddleware.cs"

}
function Add-HeathCheckApi {

    new-item -type directory -path $appFolder\Controllers\Apis -Force

    $healthCheck = @'
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    
    namespace __projectname__.Controllers
    {
        [ApiVersion("1.0", Deprecated = true)]
        [ApiVersion("2.0")]
        [Route("api/HealthCheck")]
        public class HealthCheckController : Controller
        {
    
            ILogger<HealthCheckController> _logger;
    
            public HealthCheckController(ILogger<HealthCheckController> logger)
            {
                _logger = logger;
            }
    
            [HttpGet("isalive")]
            [ProducesResponseType(200)]
            [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
            [ApiExplorerSettings(GroupName = @"HealthCheck")]
            public IActionResult GetIsAlive()
            {
                try
                {
                    return Ok();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to execute isalive");
                    return BadRequest();
                }
            }
    
    
            [HttpGet("isaliveandwell")]
            [ProducesResponseType(200)]
            [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
            [ApiExplorerSettings(GroupName = @"HealthCheck")]
            public IActionResult GetIsAliveAndWell()
            {
                try
                {
                    return Ok();
                }
                catch (Exception)
                {
                    _logger.LogError("Failed to execute isaliveandwell");
                    return BadRequest();
                }
            }
    
        }
    }
'@
    $healthCheck = $healthCheck.replace("__projectname__", $folder)
    $healthCheck | set-content  ".\Controllers\Apis\HealthCheckController.cs"


}

function Add-Dockerfile {


    $dockerlinux = @'
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS develop
# RUN apt-get update && apt-get install bash
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src/__projectname__
EXPOSE 80

FROM develop AS build
WORKDIR /src
COPY __projectname__/__projectname__.csproj __projectname__/
RUN dotnet restore __projectname__/__projectname__.csproj
COPY . .
WORKDIR /src/__projectname__
RUN dotnet build __projectname__.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish __projectname__.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "__projectname__.dll"]
'@
    $dockerlinux = $dockerlinux.replace("__projectname__", $folder)
    $dockerlinux | set-content  ".\dockerfile-linux"


    $dockerlinux = @'
'@
    $dockerlinux = $dockerlinux.replace("__projectname__", $folder)
    $dockerlinux | set-content  ".\dockerfile-windows"

    $directoryBuildprops = @'
<Project>
  <PropertyGroup>
    <DefaultItemExcludes>$(DefaultItemExcludes);$(MSBuildProjectDirectory)/obj/**/*</DefaultItemExcludes>
    <DefaultItemExcludes>$(DefaultItemExcludes);$(MSBuildProjectDirectory)/bin/**/*</DefaultItemExcludes>
  </PropertyGroup>

  <PropertyGroup Condition="'$(DOTNET_RUNNING_IN_CONTAINER)' == 'true'">
    <BaseIntermediateOutputPath>$(MSBuildProjectDirectory)/obj/container/</BaseIntermediateOutputPath>
    <BaseOutputPath>$(MSBuildProjectDirectory)/bin/container/</BaseOutputPath>
  </PropertyGroup>

  <PropertyGroup Condition="'$(DOTNET_RUNNING_IN_CONTAINER)' != 'true'">
    <BaseIntermediateOutputPath>$(MSBuildProjectDirectory)/obj/local/</BaseIntermediateOutputPath>
    <BaseOutputPath>$(MSBuildProjectDirectory)/bin/local/</BaseOutputPath>
  </PropertyGroup>
</Project>
'@
    #$directoryBuildprops = $directoryBuildprops.replace("__projectname__", $folder)
    $directoryBuildprops | set-content  ".\Directory.Build.props"

}

function Add-watchrunlaunchSettings([string]$port = 5000) {
    $jobj = Get-Content '.\Properties\launchSettings.json' -raw | ConvertFrom-Json

    $dotnetwatch = @"
        {
          "commandName": "Executable",
          "executablePath": "C:\\Program Files\\dotnet\\dotnet.exe",
          "commandLineArgs": "watch run",
          "workingDirectory": "__projectlocation__",
          "launchBrowser": true,
          "launchUrl": "https://localhost:__port__/",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          }
        }
"@

    $projectloc = get-item $appFolder
    $projectlocation = $projectloc.FullName.Replace("\", "\\")

    $dotnetwatch = $dotnetwatch.replace("__port__", $port)
    $dotnetwatch = $dotnetwatch.replace("__projectlocation__", $projectlocation)

    $jobj.profiles | add-member -Name "Kestrel (dotnet watch run)" -value (Convertfrom-Json $dotnetwatch) -MemberType NoteProperty
    $json = $jobj| ConvertTo-Json -Depth 5
    $json | set-content  '.\Properties\launchSettings.json'

}

function Add-TestApis {
    new-item -type directory -path $appFolder\Controllers\Apis -Force

    $ProductV1 = @'
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace __projectname__.Controllers
{

    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/Product")]
    //[Route("api/v{version:apiVersion}/Product")]
    [ApiController]
    public class ProductV1Controller : ControllerBase
    {

        ILogger<ProductV1Controller> logger;
        private IMapper mapper;

        public ProductV1Controller(ILogger<ProductV1Controller> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // var repos = repo.GetAll().Select(mapper.Map<ProductInfo,ProductInfoDtoV1>);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute GET " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] object model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Invalid format");
            }

            try
            {
                // var result = mapper.Map<ProductInfo>(model);
                // repo.Add(result);
                // repo.SaveChanges();
                return Created("", null);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute POST " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] object model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest("Invalid format");
            }

            try
            {
                // var result = mapper.Map<ProductInfo>(model);
                // repo.Update(result);
                // repo.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute PUT " + ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to execute DELETE " + ex.Message);
                return BadRequest();
            }
        }
    }
}
'@
    $ProductV1 = $ProductV1.replace("__projectname__", $folder)
    $ProductV1 | set-content  ".\Controllers\Apis\ProductV1.cs"


    $ProductV2 = @'
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    namespace __projectname__.Controllers
    {
        [ApiVersion("2.0")]
        [Route("api/Product")]
        //[Route("api/v{version:apiVersion}/Product")]
        [ApiController]
        public class ProductV2Controller : ControllerBase
        {
            [HttpGet]
            public string Get() => "Hello world v2!";
        }
    }
'@
    $ProductV2 = $ProductV2.replace("__projectname__", $folder)
    $ProductV2 | set-content  ".\Controllers\Apis\ProductV2.cs"


    $AutoMapperProfile = @'
using AutoMapper;
// using __projectname__.Dto;
// using __projectname__.Model.Product;

namespace __projectname__
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //  CreateMap<ProductInfo, ProductInfoV1>()
            //  .ForMember(dest=>dest.)
            //CreateMap<ProductInfoDtoV1, ProductInfo>();
            // .ForMember(dest=>dest.useTabs, opt=>opt )
            //.ForMember(dest => dest., opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            //CreateMap<ProductInfo, ProductInfoV1>().ForMember(dest => dest., opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
'@
    $AutoMapperProfile = $AutoMapperProfile.replace("__projectname__", $folder)
    $AutoMapperProfile | set-content  ".\AutoMapperProfile.cs"    
}
function Remove-project {
    Remove-Item -Path "$appFolder\*" -Recurse
    Set-Location -Path $dir
}

# function build-project {
#     Set-Location -Path $appRootFolder
#     new-dotnet
#     # dotnet add package "WebPush-netcore"
#     # dotnet add package "Microsoft.AspNetCore.Identity" -v 2.1.0
#     # dotnet add package "Microsoft.AspNetCore.Identity.UI" -v 2.1.0
#     # dotnet add package "Microsoft.AspNetCore.Identity.EntityFrameworkCore" -v 2.1.0
#     # dotnet add package "Swashbuckle.AspNetCore"
#     Add-PushNotificationController
#     Add-TokenController
#     Add-SignalR
#     dotnet restore
#     dotnet build
#     Add-cors-swagger-startupcs
# }
# dotnet add package IdentityServer4
# dotnet add package IdentityModel