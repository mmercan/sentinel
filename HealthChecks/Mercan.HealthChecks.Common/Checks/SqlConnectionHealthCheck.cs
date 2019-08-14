using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Mercan.HealthChecks.Common.Checks
{
    public static partial class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder SqlConnectionHealthCheck(this IHealthChecksBuilder builder, string connectionString)
        {
            // return builder.AddCheck($"SqlConnectionHealthCheck {connectionString}", new SqlConnectionHealthCheck(connectionString));
            return builder.AddTypeActivatedCheck<SqlConnectionHealthCheck>($"SqlConnectionHealthCheck {connectionString}", null, null, connectionString);
        }
        public static IHealthChecksBuilder SqlConnectionHealthCheck(this IHealthChecksBuilder builder, string connectionString, string testQuery)
        {
            //return builder.AddCheck($"SqlConnectionHealthCheck {connectionString} + {testQuery}", new SqlConnectionHealthCheck(connectionString, testQuery));
            return builder.AddTypeActivatedCheck<SqlConnectionHealthCheck>($"SqlConnectionHealthCheck {connectionString} + {testQuery}", null, null, connectionString, testQuery);
        }
    }
    public class SqlConnectionHealthCheck : DbConnectionHealthCheck
    {
        private static readonly string DefaultTestQuery = "Select 1";
        private ILogger<SqlConnectionHealthCheck> logger;

        public SqlConnectionHealthCheck(string connectionString)
            : this(connectionString, testQuery: DefaultTestQuery)
        {
            // this.logger = logger;
        }


        // public SqlConnectionHealthCheck(ILogger<SqlConnectionHealthCheck> logger, string connectionString)
        //     : this(connectionString, testQuery: DefaultTestQuery)
        // {
        //     this.logger = logger;
        // }

        public SqlConnectionHealthCheck(string connectionString, string testQuery)
            : base(connectionString, testQuery ?? DefaultTestQuery)
        {
        }

        protected override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}