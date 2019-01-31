using System.Threading;
using System.Threading.Tasks;

namespace Sentinel.Handler.Comms.ScheduledTask
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}