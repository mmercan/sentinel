using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sentinel.UI.Sts
{
    public class Program
    {
        public static void Main(string[] args)
        {
#pragma warning disable CS1591
            CreateWebHostBuilder(args).Build().Run();
#pragma warning restore CS1591
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
