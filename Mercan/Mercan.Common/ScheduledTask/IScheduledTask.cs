using System.Threading;
using System.Threading.Tasks;

namespace Mercan.Common.ScheduledTask
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}