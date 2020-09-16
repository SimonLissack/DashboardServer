using DashboardServer.Services.LanMonitor.Domain.Ssh;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DashboardServer.WebApi
{
    public static class DashboardConfigurationInstallationExtensions
    {
        const string SshConfigurationSection = "LanMonitor:SshConfiguration";
        public static IServiceCollection InstallConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddConfiguration<SshConfiguration>(configuration, SshConfigurationSection);

            return serviceCollection;
        }

        static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection serviceCollection, IConfiguration configuration, string configurationSection) where TConfiguration : class, new()
        {
            var config = new TConfiguration();
            configuration.Bind(configurationSection, config);

            serviceCollection.AddSingleton(config);

            return serviceCollection;
        }
    }
}
