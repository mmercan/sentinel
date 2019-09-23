// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Caching.Distributed;
// using Microsoft.Extensions.Options;
// using Microsoft.Extensions.DependencyInjection;
// using Confluent.Kafka;
// using System.Reflection;
// using Sentinel.Model.Product.Dto;
// using System.Net;
// using StackExchange.Profiling;
// using System.Threading;

// namespace Sentinel.Api.Product.Controllers
// {
//     [ApiVersion("1.0", Deprecated = true)]
//     [ApiVersion("2.0")]
//     [Route("api/DemoProfiling")]
//     [ApiController]
//     // [ApiExplorerSettings(GroupName = @"Health Check")]
//     // [Authorize]
//     public class DemoProfilingController : Controller
//     {

//         [HttpGet]
//         public IEnumerable<string> Get()
//         {
//             string url1 = string.Empty;
//             string url2 = string.Empty;
//             using (MiniProfiler.Current.Step("Get method"))
//             {
//                 using (MiniProfiler.Current.Step("Prepare data"))
//                 {
//                     using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config"))
//                     {
//                         // Simulate a SQL call
//                         Thread.Sleep(500);
//                         url1 = "https://google.com";
//                         url2 = "https://stackoverflow.com/";
//                     }
//                 }
//                 using (MiniProfiler.Current.Step("Use data for http call"))
//                 {
//                     using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url1))
//                     {
//                         using (var client = new WebClient())
//                         {
//                             var reply = client.DownloadString(url1);
//                         }
//                     }

//                     using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url2))
//                     {
//                         using (var client = new WebClient())
//                         {
//                             var reply = client.DownloadString(url2);
//                         }
//                     }
//                 }
//             }
//             return new string[] { "value1", "value2" };
//         }
//     }
// }