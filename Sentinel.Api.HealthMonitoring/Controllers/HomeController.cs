// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Sentinel.Api.HealthMonitoring.Models;

// namespace Sentinel.Api.HealthMonitoring.Controllers
// {
//     [Route("Home")]
//     // [Authorize(AuthenticationSchemes = "azure")]

//     public class HomeController : Controller
//     {
//         [Authorize]
//         [Route("Index")]
//         public IActionResult Index()
//         {
//             return View();
//         }

//         [Route("Privacy")]
//         public string Privacy()
//         {
//             var timezone = System.TimeZoneInfo.Local.StandardName;
//             return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " " + timezone + " Matt";
//         }

//         [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//         [Route("Error")]
//         public IActionResult Error()
//         {
//             return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//         }
//     }
// }
