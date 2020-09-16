using DashboardServer.Services.LanMonitor.Domain.Router;
using DashboardServer.Services.LanMonitor.Infrastructure.Ssh;
using System.Collections.Generic;
using System.IO;

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
            using var client = _sshClientFactory.CreateScpClient();
            using var stream = new MemoryStream();
            using var streamReader = new StreamReader(stream);

            client.Connect();
            client.Download(LeasesFile, stream);
            stream.Seek(0, SeekOrigin.Begin);


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
