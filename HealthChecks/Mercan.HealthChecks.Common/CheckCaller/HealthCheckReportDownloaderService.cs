using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Common.CheckCaller
{
    public class HealthCheckReportDownloaderService
    {
        private HttpClient client;
        private string url;

        public HealthCheckReportDownloaderService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> DownloadAsync(string url)
        {
            var getitem = await client.GetAsync(url);

            if (!getitem.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed");
            }

            var content = await getitem.Content.ReadAsStringAsync();
            return content;
        }
    }
}