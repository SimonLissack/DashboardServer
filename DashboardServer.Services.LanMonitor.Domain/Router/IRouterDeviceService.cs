using System.Collections.Generic;

namespace DashboardServer.Services.LanMonitor.Domain.Router
{
    public interface IRouterDeviceService
    {
        IAsyncEnumerable<RouterDevice> ListAllDevices();
        IAsyncEnumerable<RouterDevice> ListActiveDevices();
    }
}
