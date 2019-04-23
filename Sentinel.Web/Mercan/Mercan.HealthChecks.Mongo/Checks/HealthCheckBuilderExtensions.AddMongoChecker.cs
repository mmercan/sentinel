using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Mercan.HealthChecks.Mongo
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddMongoHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            return builder.AddCheck($"MongoHealthCheck {connectionString}", new MongoHealthCheck(connectionString));
        }
        public static IHealthChecksBuilder AddMongoHealthCheck(this IHealthChecksBuilder builder, string connectionString, string databaseName)
        {
            return builder.AddCheck($"MongoHealthCheck {connectionString} {databaseName}", new MongoHealthCheck(connectionString, databaseName));
        }
    }
    public class MongoHealthCheck : IHealthCheck
    {
        public IMongoDatabase MongoDb { get; private set; }
        public MongoClient mongoClient { get; private set; }
        public string IdFieldName { get; private set; }
        private string collectionName;
        public static readonly string HealthCheckName = "MongoHealthCheck";
        private string connectionString;
        private string databaseName;
        public MongoHealthCheck(string connectionString, string databaseName)
        {
            this.connectionString = connectionString;
            this.databaseName = databaseName;
        }
        public MongoHealthCheck(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                IDictionary<string, Object> data = new Dictionary<string, object>();
                data.Add("type", "MongoHealthCheck");
                try
                {
                    mongoClient = new MongoClient(connectionString);

                    var server = mongoClient.Settings.Server.Host;

                    var servers = string.Join(",", mongoClient.Settings.Servers.Select(p => p.Host));
                    if (!string.IsNullOrWhiteSpace(servers))
                    {
                        data.Add("servers", servers);
                    }

                    var databases = mongoClient.ListDatabaseNames();
                    foreach (var database in databases.ToList())
                    {
                        data.Add(database, database);
                    }
                    // MongoDb = mongoClient.GetDatabase(databaseName);
                    string description = $"MongoDd {connectionString} is healthy";
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Healthy(description, rodata);
                }
                catch (Exception ex)
                {
                    string description = "Mongo is failed with exception " + ex.Message;
                    ReadOnlyDictionary<string, Object> rodata = new ReadOnlyDictionary<string, object>(data);
                    return HealthCheckResult.Unhealthy(description, data: rodata);
                }
            });
        }
    }
}