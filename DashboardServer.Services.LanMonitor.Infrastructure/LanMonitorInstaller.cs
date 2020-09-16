using DashboardServer.Services.LanMonitor.Domain.Router;
using DashboardServer.Services.LanMonitor.Infrastructure.Router;
using DashboardServer.Services.LanMonitor.Infrastructure.Ssh;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DashboardServer.Services.LanMonitor.Infrastructure
{
    public static class LanMonitorInstaller
    {
        public static IServiceCollection InstallLanMonitorService(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddTransient<SshClientFactory, SshClientFactory>()
                .AddTransient<IDnsMasqLeaseService, DnsMasqLeaseService>();

            return serviceCollection;
        }
    }
}
