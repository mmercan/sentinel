using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sentinel.Api.HealthMonitoring.Models;

namespace Sentinel.Api.HealthMonitoring.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> logger;
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {

            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                logger.LogCritical("Is Not Authenticated");
                return Ok("Is NOT Authenticated");
            }
            else
            {
                logger.LogCritical("Is  Authenticated");
                return Ok("Is  Authenticated");
            }
        }
        public IActionResult Privacy()
        {
            return Ok(User.Identity);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
