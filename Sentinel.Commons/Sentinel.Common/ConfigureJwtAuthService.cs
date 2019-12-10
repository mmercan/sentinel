using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Sentinel.Common
{
    public static class JwtAuthService
    {
        public static void ConfigureJwtAuthService(this IServiceCollection services, IConfiguration Configuration)
        {
            var audienceConfig = Configuration.GetSection("Tokens");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],
                // Validate the token expiry
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })



          .AddJwtBearer("azure", cfg =>
          {
              cfg.RequireHttpsMetadata = false;
              cfg.SaveToken = true;
              cfg.Authority = Configuration["AzureAd:Instance"] + "/" + Configuration["AzureAD:TenantId"];
              cfg.Audience = Configuration["AzureAd:ClientId"];

              cfg.Events = new JwtBearerEvents
              {
                  OnMessageReceived = context =>
                  {
                      var accessToken = context.Request.Query["access_token"];
                      // If the request is for our hub...
                      var path = context.HttpContext.Request.Path;
                      if (!string.IsNullOrEmpty(accessToken) // && (path.StartsWithSegments("/hubs/chat"))
                          )
                      { context.Token = accessToken; }
                      return Task.CompletedTask;
                  }
              };
          })
            .AddJwtBearer("sts", cfg =>
             {
                 cfg.TokenValidationParameters = tokenValidationParameters;

                 cfg.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var accessToken = context.Request.Query["access_token"];

                         // If the request is for our hub...
                         var path = context.HttpContext.Request.Path;
                         if (!string.IsNullOrEmpty(accessToken) //   && (path.StartsWithSegments("/hubs/chat"))
                             )
                         { context.Token = accessToken; }
                         return Task.CompletedTask;
                     }
                 };

             });
            //use both jwt schemas interchangeably  https://stackoverflow.com/questions/49694383/use-multiple-jwt-bearer-authentication
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().AddAuthenticationSchemes("azure", "sts").Build();
            });
        }
    }
}