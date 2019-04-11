using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Mercan.Common.Mongo;
using Mercan.Common;
using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;
using System.Reflection;
using Sentinel.Model.Product.Dto;

namespace Sentinel.Api.Product.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/HealthCheck")]
    [ApiExplorerSettings(GroupName = @"Health Check")]
   // [Authorize]
    public class HealthCheckController : Controller
    {
        ILogger<HealthCheckController> _logger;
        private KafkaListener<TriggerHandler> kafkahandler;
        private IServiceCollection services;
        public HealthCheckController(IServiceCollection services, ILogger<HealthCheckController> logger, IDistributedCache cache, IOptions<MangoBaseRepoSettings> mangoBaseRepoSettings,
         MangoBaseRepo<ProductInfoDtoV2> repo, KafkaListener<TriggerHandler> kafkahandler)
        {
            _logger = logger;
            this.kafkahandler = kafkahandler;
            this.services = services;
            MethodInfo inf = null;
            //  inf.GetCustomAttributes(typeof(KafkaListenerAttribute), false)
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
                this.kafkahandler.Listen();
                 return Ok("Product Api is Alive");
            }
            catch (Exception ex)
            {

                _logger.LogError("Failed to execute isalive " + ex.Message);
                return BadRequest();
            }
        }

        // private void Consume(string GroupId, string BootstrapServers, string topic, MethodInfo methodinfo)
        // {
        //     var conf = new ConsumerConfig
        //     {
        //         GroupId = "test-consumer-group",
        //         BootstrapServers = "localhost:9092",
        //         // Note: The AutoOffsetReset property determines the start offset in the event
        //         // there are not yet any committed offsets for the consumer group for the
        //         // topic/partitions of interest. By default, offsets are committed
        //         // automatically, so in this example, consumption will only start from the
        //         // eariest message in the topic 'my-topic' the first time you run the program.
        //         AutoOffsetReset = AutoOffsetResetType.Earliest
        //     };
        //     using (var c = new Consumer<Ignore, string>(conf))
        //     {
        //         c.Subscribe("my-topic");
        //         bool consuming = true;
        //         // The client will automatically recover from non-fatal errors. You typically
        //         // don't need to take any action unless an error is marked as fatal.
        //         c.OnError += (_, e) => consuming = !e.IsFatal;
        //         while (consuming)
        //         {
        //             try
        //             {
        //                 var cr = c.Consume();
        //                 //cr.Topic
        //                 Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
        //             }
        //             catch (ConsumeException e)
        //             {
        //                 Console.WriteLine($"Error occured: {e.Error.Reason}");
        //             }
        //         }
        //         // Ensure the consumer leaves the group cleanly and final offsets are committed.
        //         c.Close();
        //     }
        // }


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
