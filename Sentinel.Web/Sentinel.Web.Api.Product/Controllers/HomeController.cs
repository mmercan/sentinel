using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mercan.Common.Mongo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sentinel.Web.Api.Product.Models;

namespace Sentinel.Web.Api.Product.Controllers
{
    public class HomeController : Controller
    {
        private IDistributedCache cache;

        public HomeController(IDistributedCache cache, IOptions<MangoBaseRepoSettings> mangoBaseRepoSettings, ILogger<HomeController> logger)
        {
            this.cache = cache;

            logger.LogCritical("CollectionName");
            logger.LogCritical(mangoBaseRepoSettings.Value.CollectionName);

        }
        public IActionResult Index()
        {
            cache.SetString("call", DateTime.Now.ToLongTimeString());
            cache.SetString("date", DateTime.Now.ToLongDateString());
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
