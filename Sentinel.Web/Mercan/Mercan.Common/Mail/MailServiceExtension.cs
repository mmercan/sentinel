using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Mercan.Common.Mail;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MailServiceExtension
    {


        public static IServiceCollection AddMailService(
               this IServiceCollection serviceCollection,
               MailServiceSettings options,
               ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
               ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {

            //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
            serviceCollection.Configure<MailServiceSettings>(o => o = options);
            serviceCollection.AddSingleton<MailService>();
            return serviceCollection;
        }



        public static IServiceCollection AddMailService(
        this IServiceCollection serviceCollection,
       IConfiguration options,
       ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
       ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {

            //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
            serviceCollection.Configure<MailServiceSettings>(options);
            serviceCollection.AddSingleton<MailService>();
            return serviceCollection;
        }

        public static IServiceCollection AddMailService(
        this IServiceCollection serviceCollection,
       ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
       ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {

            //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
            serviceCollection.AddSingleton<MailService>();
            return serviceCollection;
        }

    }

}