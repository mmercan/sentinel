using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Azure.ServiceBus;

namespace Mercan.HealthChecks.ServiceBus
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddServiceBusHealthCheck(this IHealthChecksBuilder builder, string connectionString, string entityPath)
        {
            return builder.AddCheck($"ServiceBusHealthCheck {connectionString}", new ServiceBusHealthCheck(connectionString, entityPath));
        }

        public static IHealthChecksBuilder AddServiceBusHealthCheck(this IHealthChecksBuilder builder, string nameSpace, string topicName, string AccessPolicyName, string accessPolicyKey)
        {
            return builder.AddCheck($"ServiceBusHealthCheck namespace:{nameSpace} topic:{topicName}", new ServiceBusHealthCheck(nameSpace, topicName, AccessPolicyName, accessPolicyKey));
        }


        public static IHealthChecksBuilder AddServiceBusHealthCheck(this IHealthChecksBuilder builder, string nameSpace, string topicName, string subscriptionName, string AccessPolicyName, string accessPolicyKey)
        {
            return builder.AddCheck($"ServiceBusHealthCheck namespace:{nameSpace} topic:{topicName}", new ServiceBusHealthCheck(nameSpace, topicName, AccessPolicyName, accessPolicyKey, subscriptionName));
        }

    }
    public enum ServiceBusHealthCheckType
    {
        Receive,
        Send
    }

    public class ServiceBusHealthCheck : IHealthCheck
    {
        string connectionString;
        string SendConnectionString;
        string ListenConnectionString;
        string nameSpace = "";
        string topicName;
        private ServiceBusHealthCheckType serviceBusHealthCheckType;
        string subscriptionName;

        public ServiceBusHealthCheck(string nameSpace, string topicName, string AccessPolicyName, string accessPolicyKey)
        {
            this.connectionString = $"Endpoint=sb://{nameSpace}.servicebus.windows.net/;SharedAccessKeyName={AccessPolicyName};SharedAccessKey={accessPolicyKey};TransportType=AmqpWebSockets";
            this.SendConnectionString = connectionString;
            this.ListenConnectionString = connectionString;
            this.topicName = topicName;
            this.serviceBusHealthCheckType = ServiceBusHealthCheckType.Send;
            this.nameSpace = nameSpace;
        }


        public ServiceBusHealthCheck(string nameSpace, string topicName, string AccessPolicyName, string accessPolicyKey, string subscriptionName)
        {
            this.connectionString = $"Endpoint=sb://{nameSpace}.servicebus.windows.net/;SharedAccessKeyName={AccessPolicyName};SharedAccessKey={accessPolicyKey};TransportType=AmqpWebSockets";
            this.SendConnectionString = connectionString;
            this.ListenConnectionString = connectionString;
            this.topicName = topicName;
            this.subscriptionName = subscriptionName;
            this.serviceBusHealthCheckType = ServiceBusHealthCheckType.Receive;
            this.nameSpace = nameSpace;
        }

        public ServiceBusHealthCheck(string connectionString, string entityPath)
        {
            this.SendConnectionString = connectionString;
            this.ListenConnectionString = connectionString;
            this.topicName = entityPath;
            this.serviceBusHealthCheckType = ServiceBusHealthCheckType.Send;
            if (connectionString != null)
            {
                this.nameSpace = connectionString.Split('.')[0];
            }
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "ServiceBusHealthCheck");
                // data.Add("connectionString", this.connectionString);
                data.Add("topicName", this.topicName);
                try
                {
                    if (serviceBusHealthCheckType == ServiceBusHealthCheckType.Receive)
                    {
                        data.Add("subscriptionName", this.subscriptionName);
                        var subscriptionClient = new SubscriptionClient(ListenConnectionString, topicName, subscriptionName);
                        var clientid = subscriptionClient.ClientId;
                        data.Add("clientid", clientid);

                        var rulestask = subscriptionClient.GetRulesAsync();
                        rulestask.Wait();
                        data.Add("rules", string.Join(",", rulestask.Result));
                    }
                    else
                    {
                        var topicClient = new QueueClient(SendConnectionString, topicName);
                        var clientid = topicClient.ClientId;
                        data.Add("clientid", clientid);
                        var list = topicClient.Path;

                        Message mes = new Message();
                        mes.Body = System.Text.Encoding.UTF8.GetBytes("TestMessage");

                        var sendtask = topicClient.ScheduleMessageAsync(mes, DateTime.Now.AddDays(100));
                        sendtask.Wait();

                        var cancell = topicClient.CancelScheduledMessageAsync(sendtask.Result);
                        cancell.Wait();
                    }

                    string description = $"ServiceBus namespace {nameSpace}.servicebus.windows.net is healthy";
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (Exception ex)
                {
                    string description = "ServiceBus is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }
            });
        }
    }
}