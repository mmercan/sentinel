using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Mercan.HealthChecks.Common.Checks
{
    public abstract class DbConnectionHealthCheck : IHealthCheck
    {
        protected DbConnectionHealthCheck(string connectionString)
            : this(connectionString, testQuery: null)
        {
        }

        protected DbConnectionHealthCheck(string connectionString, string testQuery)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            TestQuery = testQuery;
        }

        protected string ConnectionString { get; }

        // This sample supports specifying a query to run as a boolean test of whether the database
        // is responding. It is important to choose a query that will return quickly or you risk
        // overloading the database.
        //
        // In most cases this is not necessary, but if you find it necessary, choose a simple query such as 'SELECT 1'.
        protected string TestQuery { get; }

        protected abstract DbConnection CreateConnection(string connectionString);

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            using (var connection = CreateConnection(ConnectionString))
            {
                try
                {
                    data.Add("database", connection.Database);
                    data.Add("dataSource", connection.DataSource);
                    data.Add("type", "DbConnection");
                    data.Add("dbtype", connection.GetType().Name);

                    await connection.OpenAsync(cancellationToken);

                    if (TestQuery != null)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = TestQuery;

                        await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                catch (DbException ex)
                {
                    return new HealthCheckResult(status: HealthStatus.Unhealthy, exception: ex, data: data);
                    // return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex,data: data);
                }
            }

            return HealthCheckResult.Healthy("DbConnection is Healhty", data);
        }
    }
}