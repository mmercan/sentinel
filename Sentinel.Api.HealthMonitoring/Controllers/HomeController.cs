using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentinel.Api.HealthMonitoring.Models;

namespace Sentinel.Api.HealthMonitoring.Controllers
{
    [Route("Home")]
    // [Authorize(AuthenticationSchemes = "azure")]

    public class HomeController : Controller
    {
        [Authorize]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(AuthenticationSchemes = "azure")]
        [Route("Indexazure")]
        public string Indexazure()
        {
            return "azureeeee";
        }

        [Route("Privacy")]
        public string Privacy()
        {
            return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
