using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Mercan.Common.Mongo;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MangoBaseRepoExtension
    {


        public static IServiceCollection AddMangoRepo<T>(
               this IServiceCollection serviceCollection,
               MangoBaseRepoSettings options,
               ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
               ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where T : new()
        {

            //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
            serviceCollection.Configure<MangoBaseRepoSettings>(o => o = options);
            serviceCollection.AddSingleton<MangoBaseRepo<T>>();
            return serviceCollection;
        }



        public static IServiceCollection AddMangoRepo<T>(
        this IServiceCollection serviceCollection,
       IConfiguration options,
       ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
       ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where T : new()
        {

            //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
            serviceCollection.Configure<MangoBaseRepoSettings>(options);
            serviceCollection.AddSingleton<MangoBaseRepo<T>>();
            return serviceCollection;
        }

        public static IServiceCollection AddMangoRepo<T>(
        this IServiceCollection serviceCollection,
       ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
       ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where T : new()
        {

            //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
            serviceCollection.AddSingleton<MangoBaseRepo<T>>();
            return serviceCollection;
        }

    }

}