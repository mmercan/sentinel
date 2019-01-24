using System;
using System.Threading;
using NATS.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sentinel.Model.Product;
using EasyNetQ;

namespace Sentinel.Handler.Comms
{
    public class ProductChangeSubscribe
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private const string QUEUE_GROUP = "save-handler";
        ILogger<ProductChangeSubscribe> logger;
        IConfiguration configuration;


        public ProductChangeSubscribe(ILogger<ProductChangeSubscribe> logger, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.logger = logger;
            this.configuration = configuration;

        }

        public void Subscribe()
        {
            try
            {
                logger.LogCritical("RabbitmqSubscribe to message queue url: {0}", configuration["NATS_URL"]);
                Console.WriteLine("RabbitmqSubscribe to message queue url: {0}", configuration["NATS_URL"]);
                using (var bus = RabbitHutch.CreateBus("host=sentinel-service-rabbitmq;username=rabbitmq;password=rabbitmq"))
                {
                    //bus.
                    bus.Subscribe<ProductInfo>("product", Handler, x => x.WithTopic("product.newproduct"));

                    // Console.WriteLine("Listening for (payment.*) messages. Hit <return> to quit.");
                    // Console.ReadLine();
                    // Console.WriteLine("terminated");
                }

            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("Exception: " + ex.Message);
                System.Console.Error.WriteLine(ex);
            }
        }

        public void Handler(ProductInfo product)
        {

            // logger.LogCritical("New Product Info" + product.Id);
            // if (payment is CardPayment cardPayment)
            // {
            //     Console.WriteLine("Processing Card Payment = <" +
            //                       cardPayment.CardNumber + ", " +
            //                       cardPayment.CardHolderName + ", " +
            //                       cardPayment.ExpiryDate + ", " +
            //                       cardPayment.Amount + ">");
            // }

            // if (payment is PurchaseOrder purchaseOrder)
            // {
            //     Console.WriteLine("Processing Purchase Order = <" +
            //                       purchaseOrder.CompanyName + ", " +
            //                       purchaseOrder.PoNumber + ", " +
            //                       purchaseOrder.PaymentDayTerms + ", " +
            //                       purchaseOrder.Amount + ">");
            // }
        }
    }
}