using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mercan.Common;
using Mercan.Common.Mongo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sentinel.Api.Product.Models;
using Sentinel.Model.Product.Dto;

namespace Sentinel.Api.Product.Controllers
{
    public class HomeController : Controller
    {
        private IDistributedCache cache;

        public HomeController(IDistributedCache cache, IOptions<MangoBaseRepoSettings> mangoBaseRepoSettings,
         ILogger<HomeController> logger, MangoBaseRepo<ProductInfoDtoV2> repo, TriggerHandler triggerHandler)
        {
            this.cache = cache;
            // var rr = repo.GetAll();

            // ProductInfoDtoV2 blah = new ProductInfoDtoV2 { Name = "my name" };
            // var addtask = repo.AddAsync(blah);
            // addtask.Wait();

            // logger.LogCritical("CollectionName");
            // logger.LogCritical(mangoBaseRepoSettings.Value.CollectionName);

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
