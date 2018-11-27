using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Confluent.Kafka;


namespace Mercan.Common
{
    public class KafkaListener<T>
    {
        private ILogger<KafkaListener<T>> logger;
        private T myobject;
        private IOptions<KafkaListenerSettings> kafkaLisSetsOptions;

        public KafkaListener(T myobject, IOptions<KafkaListenerSettings> kafkaLisSetsOptions, ILogger<KafkaListener<T>> logger)
        {
            this.logger = logger;
            this.myobject = myobject;
            this.kafkaLisSetsOptions = kafkaLisSetsOptions;
            // logger.LogDebug("KafkaListener Constructed");
            // if (myobject == null)
            // {
            //     logger.LogCritical("===================== T instance is is null");
            // }
            // else
            // {
            //     logger.LogCritical("===================== T instance is " + myobject.GetType().FullName);
            // }
        }

        public void Listen()
        {
            if (myobject == null)
            {
                logger.LogCritical("===================== T instance is is null");
            }
            else
            {
                logger.LogCritical("===================== T instance " + myobject.GetType().FullName);

                var methodInfos = myobject.GetType().GetMethods().Where(m => m.GetCustomAttributes(typeof(KafkaListenerAttribute), false).Length > 0).ToList();
                logger.LogCritical("number of attributes " + methodInfos.Count());
                foreach (var methodinfo in methodInfos)
                {

                    methodinfo.Invoke(myobject, null);
                }
            }
        }



        private void Consume(string GroupId, string BootstrapServers, string topic, MethodInfo methodinfo)
        {

            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // eariest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetResetType.Earliest
            };

            using (var c = new Consumer<Ignore, string>(conf))
            {
                c.Subscribe("my-topic");

                bool consuming = true;
                // The client will automatically recover from non-fatal errors. You typically
                // don't need to take any action unless an error is marked as fatal.
                c.OnError += (_, e) => consuming = !e.IsFatal;

                while (consuming)
                {
                    try
                    {
                        var cr = c.Consume();
                        Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }

                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                c.Close();
            }
        }
    }

}
