using System;
using System.Threading;
using NATS.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sentinel.Handler.Comms
{
    public class NatsSubscribe
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private const string QUEUE_GROUP = "save-handler";

        public void Subscribe()
        {

        }
        public NatsSubscribe(ILogger<NatsSubscribe> logger, IConfiguration Configuration, ILoggerFactory loggerFactory)
        {
            try
            {
                // var logger = loggerFactory.CreateLogger<NatsSubscribe>();

                logger.LogError("LogError Connecting to message queue url: {0}", Configuration["NATS_URL"]);
                Console.WriteLine(" WriteLine Connecting to message queue url: {0}", Configuration["NATS_URL"]);
                using (var connection = new ConnectionFactory().CreateConnection(Configuration["NATS_URL"]))
                {
                    var subscription = connection.SubscribeAsync("foo", QUEUE_GROUP);
                    subscription.MessageHandler += SaveViewer;
                    subscription.Start();
                    Console.WriteLine("Listening on subject: {0}, queue: {1}", "foo", QUEUE_GROUP);

                    _ResetEvent.WaitOne();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("Exception: " + ex.Message);
                System.Console.Error.WriteLine(ex);
            }
        }


        private static void SaveViewer(object sender, MsgHandlerEventArgs e)
        {
            Console.WriteLine("Received message {0}", e.Message);
            // var eventMessage = MessageHelper.FromData<ViewerSignedUpEvent>(e.Message.Data);
            //  Console.WriteLine("Saving new viewer, signed up at: {0}; event ID: {1}", eventMessage.SignedUpAt, eventMessage.CorrelationId);

            // var viewer = eventMessage.Viewer;
            // using (var context = new WebinarContext())
            // {
            //     //reload child objects:
            //     viewer.Country = context.Countries.Single(x => x.CountryCode == viewer.Country.CountryCode);
            //     viewer.Role = context.Roles.Single(x => x.RoleCode == viewer.Role.RoleCode);
            //     var interestCodes = viewer.Interests.Select(y => y.InterestCode);
            //     viewer.Interests = context.Interests.Where(x => interestCodes.Contains(x.InterestCode)).ToList();

            //     context.Viewers.Add(viewer);
            //     context.SaveChanges();
            // }
            // Console.WriteLine("Viewer saved. Viewer ID: {0}; event ID: {1}", eventMessage.Viewer.ViewerId, eventMessage.CorrelationId);
        }

    }
}