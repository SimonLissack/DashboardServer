using DashboardServer.Services.LanMonitor.Domain.Router;
using DashboardServer.Services.LanMonitor.Domain.Ssh;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardServer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeasesController : ControllerBase
    {
        private readonly IDnsMasqLeaseService _dnsMasqLeaseService;
        private readonly SshConfiguration _sshConfiguration;

        public LeasesController(IDnsMasqLeaseService dnsMasqLeaseService, SshConfiguration sshConfiguration)
        {
            _dnsMasqLeaseService = dnsMasqLeaseService;
            _sshConfiguration = sshConfiguration;
        }

        [HttpGet]
        public async Task<LeasesRepresentation> Get()
        {
            var leases = _dnsMasqLeaseService.ListCurrentLeases();

            return new LeasesRepresentation
            {
                HostName = _sshConfiguration.HostName,
                Leases = await leases.ToListAsync()
            };
        }

        public class LeasesRepresentation
        {
            public string HostName { get; set; }
            public List<DnsMasqLease> Leases { get; set; }
        }
    }
}
