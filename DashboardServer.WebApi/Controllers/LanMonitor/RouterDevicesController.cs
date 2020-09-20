using DashboardServer.Services.LanMonitor.Domain.Router;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardServer.WebApi.Controllers.LanMonitor
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouterDevicesController : ControllerBase
    {
        private readonly IRouterDeviceService _routerDeviceService;

        public RouterDevicesController(IRouterDeviceService routerDeviceService)
        {
            _routerDeviceService = routerDeviceService;
        }

        [HttpGet]
        [Route("active")]
        public async Task<List<RouterDevice>> Active()
        {
            var devices = _routerDeviceService.ListActiveDevices();

            return await devices.ToListAsync();
        }

        [HttpGet]
        public async Task<List<RouterDevice>> Get()
        {
            var devices = _routerDeviceService.ListAllDevices();

            return await devices.ToListAsync();
        }
    }
}
