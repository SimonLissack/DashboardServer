using System.Collections.Generic;

namespace DashboardServer.Services.LanMonitor.Infrastructure.Router
{
    /// <summary>
    /// Represents the structure in /jffs/nmp_cl_json.js
    /// </summary>
    public class AllClientsRepresentation : Dictionary<string, KnownClient>
    {
        public IEnumerable<KnownClient> Devices => Values;
    }

    public record KnownClient(int Type, string Mac, string Name, string Vendor);
}
