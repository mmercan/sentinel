using System.Threading.Tasks;
using Mercan.HealthChecks.Network.HttpRequest;
using Xunit;

namespace Mercan.HealthChecks.Network.Tests.Checks
{
    public class HttpRequestFactoryShould
    {


        public HttpRequestFactoryShould()
        {

        }

        [Fact]
        public async Task GetAsyncWorks()
        {
            await HttpRequestFactory.GetAsync("https://google.com");
        }

        [Fact]
        public void GetWorks()
        {
            HttpRequestFactory.Get("https://google.com", "", "", "", "");
        }


        [Fact]
        public async Task PostWorks()
        {
            var data = new MockData();
            await HttpRequestFactory.Post("https://jsonplaceholder.typicode.com/posts", data);
        }


        [Fact]
        public async Task PutWorks()
        {
            var data = new MockData();
            await HttpRequestFactory.Put("https://jsonplaceholder.typicode.com/posts/1", data);
        }

        [Fact]
        public async Task PatchWorks()
        {
            var data = new MockData();
            await HttpRequestFactory.Patch("https://jsonplaceholder.typicode.com/posts/1", data);
        }


        [Fact]
        public async Task DeleteWorks()
        {
            await HttpRequestFactory.Delete("https://jsonplaceholder.typicode.com/posts/1");
        }

        public void PostFileWorks()
        {

        }
    }
}