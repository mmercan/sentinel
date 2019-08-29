using Microsoft.Extensions.Configuration;

namespace Mercan.HealthChecks.ServiceBus.Tests
{
    public static class ConfigurationProvider
    {
        static IConfiguration _config;
        public static void InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test2.json", false)
                .AddJsonFile("appsettings.test.json", false)
                .Build();
            _config = config;
        }

        public static IConfiguration Config
        {
            get
            {
                if (_config == null)
                {
                    InitConfiguration();
                    return _config;
                }
                else
                {
                    return _config;
                }
            }
        }
    }
}