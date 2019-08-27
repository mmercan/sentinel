using System.Collections.Generic;

namespace Sentinel.Api.HealthMonitoring.Models
{
    public class HealthCheckDownloaderOptions
    {
        public Dictionary<string, HealthCheckDownloader> HeathChecks { get; set; }
    }

    public class HealthCheckDownloader
    {
        public string Url { get; set; }
        public string CertThumbprint { get; set; }
    }
}