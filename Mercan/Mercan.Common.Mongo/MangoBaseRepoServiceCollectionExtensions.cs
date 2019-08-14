// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using Mercan.Common.Mongo;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Configuration;

// namespace Microsoft.Extensions.DependencyInjection
// {
//     public static class MangoBaseRepoServiceCollectionExtensions
//     {


//         public static IServiceCollection AddMongoContext<T>(
//                this IServiceCollection serviceCollection,
//                ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
//                ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
//         //where TContextImplementation : DbContext, TContextService
//         {
//             return serviceCollection;
//         }


//         public static IServiceCollection AddMongoContext<T>(
//                this IServiceCollection serviceCollection,
//                MangoBaseRepoSettings options,
//                ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
//                ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
//         //where TContextImplementation : DbContext, TContextService
//         {

//             //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
//             serviceCollection.Configure<MangoBaseRepoSettings>(o => o = options);
//             return serviceCollection;
//         }


//         public static IServiceCollection AddMongoContext<T>(
//                this IServiceCollection serviceCollection,
//                Action<MangoBaseRepoSettings> options,
//                ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
//                ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
//         //where TContextImplementation : DbContext, TContextService
//         {

//             //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
//             serviceCollection.Configure<T>(options);
//             return serviceCollection;
//         }

//         public static IServiceCollection AddMongoContext<T>(
//        this IServiceCollection serviceCollection,
//        IConfiguration section,
//        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
//        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
//         //where TContextImplementation : DbContext, TContextService
//         {

//             //serviceCollection.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
//             serviceCollection.Configure<MangoBaseRepoSettings>(config: section);

//             return serviceCollection;
//         }

//         // public static IServiceCollection AddMongoContext<TContextService>(
//         //    this IServiceCollection serviceCollection,
//         //    Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
//         //    ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
//         //    ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
//         // //where TContextImplementation : DbContext, TContextService
//         // {
//         //     // Check.NotNull(serviceCollection, nameof(serviceCollection));
//         //     // if (contextLifetime == ServiceLifetime.Singleton)
//         //     // {
//         //     //     optionsLifetime = ServiceLifetime.Singleton;
//         //     // }
//         //     // if (optionsAction != null)
//         //     // {
//         //     //     CheckContextConstructors<TContextImplementation>();
//         //     // }

//         //     //AddCoreServices<TContextImplementation>(serviceCollection, optionsAction, optionsLifetime);

//         //     //serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContextService), typeof(TContextImplementation), contextLifetime));

//         //     return serviceCollection;
//         // }

//     }

// }