    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    // using Mercan.Common.Mongo;
    using Mercan.Common;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using Sentinel.Model.Product.Dto;
    
    namespace Sentinel.UI.Product.Controllers
    {
        [ApiVersion("1.0", Deprecated = true)]
        [ApiVersion("2.0")]
        [Route("api/HealthCheck")]
        public class HealthCheckController : Controller
        {
    
            ILogger<HealthCheckController> _logger;
            private IServiceCollection services;
    
            public HealthCheckController(IServiceCollection services, ILogger<HealthCheckController> logger, IDistributedCache cache)
            {
                _logger = logger;
                this.services = services;
            }
    
            /// <summary>
            /// isalive
            /// </summary>
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
                catch (Exception ex)
                {
    
                    _logger.LogError("Failed to execute isalive " + ex.Message);
                    return BadRequest();
                }
            }
    
    
            /// <summary>
            /// isaliveandwell
            /// </summary>
            [HttpGet("isaliveandwell")]
            [ProducesResponseType(200)]
            [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
            [ApiExplorerSettings(GroupName = @"HealthCheck")]
            // [Authorize]
            public IActionResult GetIsAliveAndWell()
            {
                string messages = "";
                messages += "IsAuthenticated : " + this.User.Identity.IsAuthenticated + " , \n";
    
                if (this.User.Identity.IsAuthenticated)
                {
                    messages += "AuthenticationType : " + this.User.Identity.AuthenticationType + " , \n";
                    messages += "Name   : " + this.User.Identity.Name + " , \n";
                    messages += "Claims : " + this.User.Claims.Select(p => p.Type + " : " + p.Value + " " + p.Properties.ToList().ToJSON()).ToList().ToJSON() + " , \n";
                }
    
                List<Type> exceptions = new List<Type>{
                    typeof(Microsoft.Extensions.Options.IOptions<>),
                    typeof(Microsoft.Extensions.Options.OptionsCache<>),
                    typeof(Microsoft.Extensions.Options.IOptionsSnapshot<>),
                    typeof(Microsoft.Extensions.Options.IOptionsMonitor<>),
                    typeof(Microsoft.Extensions.Options.IOptionsFactory<>),
                    typeof(Microsoft.Extensions.Logging.Configuration.ILoggerProviderConfiguration<>),
                    typeof(Mercan.Common.KafkaListener<>),
                    };
                try
                {
                    var serviceprovider = this.services.BuildServiceProvider();
                    foreach (var service in this.services)
                    {
                        try
                        {
                            bool skip = false;
                            if (exceptions.Contains(service.ServiceType))
                            {
                                skip = true;
                                messages += "Skipped : " + service.ServiceType.FullName + " , \n";
                            }
    
                            if (!skip)
                            {
                                if (service.ServiceType.ContainsGenericParameters)
                                {
                                    var t = service.ServiceType.MakeGenericType(typeof(HealthCheckController));
                                    var instance = serviceprovider.GetService(t);
                                    messages += "Success : " + instance.GetType().FullName + " , \n";
                                }
                                else
                                {
                                    var instance = serviceprovider.GetService(service.ServiceType);
                                    messages += "Success : " + instance.GetType().FullName + " , \n";
                                }
                            }
                        }
                        catch (Exception ex) { messages += "Failed : " + ex.Message + " , \n"; }
                    }
                    _logger.LogDebug(messages);
                    return Ok(messages);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to execute isaliveandwell " + messages + ex.Message);
                    return BadRequest();
                }
            }
    
        }
    }
