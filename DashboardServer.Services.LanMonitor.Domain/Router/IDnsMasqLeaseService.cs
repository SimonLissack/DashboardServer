using System.Collections.Generic;

namespace DashboardServer.Services.LanMonitor.Domain.Router
{
    public interface IDnsMasqLeaseService
    {
        IAsyncEnumerable<DnsMasqLease> ListCurrentLeases();
    }
}
