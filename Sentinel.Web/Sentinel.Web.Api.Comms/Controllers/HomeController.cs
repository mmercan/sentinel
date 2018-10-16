using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sentinel.Web.Api.Comms.Models;
using NATS.Client;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Sentinel.Web.Api.Comms.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> logger;
        private IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            new Publisher().Run(logger, configuration);
            return View();
        }


        private void sub()
        {
            // ConnectionFactory cf = new ConnectionFactory();
            // IConnection c = cf.CreateConnection("nats://localhost:4222/");

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


    public class Publisher
    {
        Dictionary<string, string> parsedArgs = new Dictionary<string, string>();

        int count = 50;
        string url = "nats://localhost:4222/"; //Defaults.Url;
        string subject = "foo";
        byte[] payload = null;

        public void Run(ILogger<HomeController> logger, IConfiguration configuration)
        {
            payload = Encoding.ASCII.GetBytes("My Message");
            Stopwatch sw = null;
            banner();

            Options opts = ConnectionFactory.GetDefaultOptions();

            if (configuration["NATS_URL"] != null)
            {
                opts.Url = configuration["NATS_URL"];
            }
            else
            {
                opts.Url = url;
            }
            logger.LogInformation("URL");
            logger.LogInformation(opts.Url);
            using (IConnection c = new ConnectionFactory().CreateConnection(opts))
            {
                sw = Stopwatch.StartNew();

                for (int i = 0; i < count; i++)
                {
                    c.Publish(subject, payload);
                }
                c.Flush();

                sw.Stop();

                logger.LogDebug("Published {0} msgs in {1} seconds ", count, sw.Elapsed.TotalSeconds);
                logger.LogDebug("({0} msgs/second).", (int)(count / sw.Elapsed.TotalSeconds));
                printStats(c, logger);

            }
        }

        private void printStats(IConnection c, ILogger<HomeController> logger)
        {
            IStatistics s = c.Stats;
            logger.LogDebug("Statistics:  ");
            logger.LogDebug("   Outgoing Payload Bytes: {0}", s.OutBytes);
            logger.LogDebug("   Outgoing Messages: {0}", s.OutMsgs);
        }

        private void usage()
        {
            System.Console.Error.WriteLine(
                "Usage:  Publish [-url url] [-subject subject] " +
                "-count [count] [-payload payload]");

            System.Environment.Exit(-1);
        }

        private void parseArgs(string[] args)
        {
            if (args == null)
                return;

            for (int i = 0; i < args.Length; i++)
            {
                if (i + 1 == args.Length)
                    usage();

                parsedArgs.Add(args[i], args[i + 1]);
                i++;
            }

            if (parsedArgs.ContainsKey("-count"))
                count = Convert.ToInt32(parsedArgs["-count"]);

            if (parsedArgs.ContainsKey("-url"))
                url = parsedArgs["-url"];

            if (parsedArgs.ContainsKey("-subject"))
                subject = parsedArgs["-subject"];

            if (parsedArgs.ContainsKey("-payload"))
                payload = Encoding.UTF8.GetBytes(parsedArgs["-payload"]);
        }

        private void banner()
        {
            System.Console.WriteLine("Publishing {0} messages on subject {1}",
                count, subject);
            System.Console.WriteLine("  Url: {0}", url);
            System.Console.WriteLine("  Payload is {0} bytes.",
                payload != null ? payload.Length : 0);
        }
    }
}
