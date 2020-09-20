using DashboardServer.Services.LanMonitor.Domain.Router;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardServer.WebApi.Controllers.LanMonitor
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeasesController : ControllerBase
    {
        private readonly IDnsMasqLeaseService _dnsMasqLeaseService;

        public LeasesController(IDnsMasqLeaseService dnsMasqLeaseService)
        {
            _dnsMasqLeaseService = dnsMasqLeaseService;
        }

        [HttpGet]
        public async Task<List<DnsMasqLease>> Get()
        {
            var leases = _dnsMasqLeaseService.ListCurrentLeases();

            return await leases.ToListAsync();
        }
    }
}
