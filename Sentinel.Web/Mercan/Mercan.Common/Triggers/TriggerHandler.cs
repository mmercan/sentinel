using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
// using MMercan.Common.Interfaces;

namespace Mercan.Common
{
    public class TriggerHandler
    {

        ILogger<TriggerHandler> logger;
        public TriggerHandler(ILogger<TriggerHandler> logger)
        {
            this.logger = logger;
            var execute = Assembly.GetExecutingAssembly();
            logger.LogDebug("=============================== TriggerHandler =======================");
            logger.LogDebug(execute.FullName);
        }

        [KafkaListenerAttribute("topic1")]
        public void Blah1()
        {
            logger.LogDebug("=============================== TriggerHandler Blah1 Triggered =======================");
        }


        [KafkaListenerAttribute("topic2")]
        public void Blah2()
        {
            logger.LogDebug("=============================== TriggerHandler Blah2 Triggered =======================");
        }


        [KafkaListenerAttribute("topic3")]
        public void Blah3()
        {
            logger.LogDebug("=============================== TriggerHandler Blah3 Triggered =======================");
        }
    }

    public static class TriggerHandlerExtension
    {

        public static IServiceCollection AddTriggerHandler<TContextService>(
            this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
                            => AddTriggerHandler<TContextService, TContextService>(serviceCollection, optionsAction == null
                            ? (Action<IServiceProvider, DbContextOptionsBuilder>)null : (p, b) => optionsAction.Invoke(b), contextLifetime, optionsLifetime);

        public static IServiceCollection AddTriggerHandler<TContextService, TContextImplementation>(
                    this IServiceCollection serviceCollection, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
                    ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
                    where TContextImplementation : TContextService
        {
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContextService), typeof(TContextImplementation), contextLifetime));

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(KafkaListener<>), typeof(KafkaListener<>), contextLifetime));



            //services.AddScoped(typeof(IStore<>), typeof(SqlStore<>));

            // var serviceProvider = serviceCollection.BuildServiceProvider();
            // //serviceProvider.GetService(typeof(TContextService));
            // var contentservice = serviceProvider.GetService<TContextService>();
            // System.Attribute[] attributes = System.Attribute.GetCustomAttributes(typeof(KafkaListener));

            // foreach (Attribute attribute in attributes)
            // {
            //     if (attribute is MenuItemAttribute)
            //     {
            //         //Get me the method info
            //         MethodInfo[] methods = attribute.GetType().GetMethods();
            //     }
            // }



            // Type thisType = typeof(TContextService);
            // MethodInfo theMethod = thisType.GetMethod(TheCommandString);
            // theMethod.Invoke(contentservice, userParameters);




            return serviceCollection;


        }

    }
}