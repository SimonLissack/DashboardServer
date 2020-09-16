using DashboardServer.Services.LanMonitor.Domain.Ssh;
using Renci.SshNet;

namespace DashboardServer.Services.LanMonitor.Infrastructure.Ssh
{
    public class SshClientFactory
    {
        private readonly SshConfiguration _configuration;

        public SshClientFactory(SshConfiguration configuration)
        {
            _configuration = configuration;
        }

        ConnectionInfo ConnectionInfo => new ConnectionInfo(_configuration.HostName, _configuration.User, new PrivateKeyAuthenticationMethod(_configuration.User, new PrivateKeyFile(_configuration.RsaKeyLocation)));

        public ScpClient CreateScpClient()
        {
            return new ScpClient(ConnectionInfo);
        }
    }
}
