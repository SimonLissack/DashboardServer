using DashboardServer.Services.LanMonitor.Domain.Router;
using DashboardServer.Services.LanMonitor.Infrastructure.Ssh;
using System.Collections.Generic;

namespace DashboardServer.Services.LanMonitor.Infrastructure.Router
{
    public class DnsMasqLeaseService : IDnsMasqLeaseService
    {
        const string LeasesFile = "/var/lib/misc/dnsmasq.leases";

        private readonly SshClientFactory _sshClientFactory;

        public DnsMasqLeaseService(SshClientFactory sshClientFactory)
        {
            _sshClientFactory = sshClientFactory;
        }

        public async IAsyncEnumerable<DnsMasqLease> ListCurrentLeases()
        {
            using var streamReader = _sshClientFactory
                .CreateScpClient()
                .OpenFileStreamReader(LeasesFile);

            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();
                yield return ParseLine(line);
            }
        }

        static DnsMasqLease ParseLine(string line)
        {
            var columns = line.Split(' ');

            return new(int.Parse(columns[0]), columns[1], columns[2], columns[3], columns[4]);
        }
    }

}
