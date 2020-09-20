using DashboardServer.Services.LanMonitor.Domain.Router;
using DashboardServer.Services.LanMonitor.Infrastructure.Ssh;
using Renci.SshNet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardServer.Services.LanMonitor.Infrastructure.Router
{
    public class RouterDeviceService : IRouterDeviceService
    {
        const string AllDevicesFile = "/jffs/nmp_cl_json.js";
        const string ActiveClientList = "/tmp/clientlist.json";
        const string DisconnectedStatus = "disconnected";

        private readonly SshClientFactory _sshClientFactory;

        public RouterDeviceService(SshClientFactory sshClientFactory)
        {
            _sshClientFactory = sshClientFactory;
        }
        public async IAsyncEnumerable<RouterDevice> ListAllDevices()
        {
            using var client = _sshClientFactory.CreateScpClient();

            var activeDevices = (await ListActiveDevices(client)).ToDictionary(d => d.MacAddress);
            var allDevices = await ListAllDevices(client);

            foreach(var device in allDevices)
            {
                activeDevices.TryGetValue(device.Mac, out var activeDevice);

                yield return new RouterDevice(
                    device.Mac,
                    activeDevice?.Ip,
                    device.Name,
                    device.Vendor,
                    activeDevice?.Interface ?? DisconnectedStatus,
                    activeDevice?.Rssi
                );
            }
        }

        public IAsyncEnumerable<RouterDevice> ListActiveDevices()
        {
            return ListAllDevices().Where(d => d.ConnectionType != DisconnectedStatus);
        }

        async Task<IEnumerable<ParsedConnectedDevice>> ListActiveDevices(ScpClient client)
        {
            using var clientListStream = client.OpenFileStream(ActiveClientList);

            var activeDevicesRepresentation = await clientListStream.DeserializeAsync<ActiveDevicesRepresentation>();

            return activeDevicesRepresentation.GetConnectedDevices();
        }

        async Task<IEnumerable<KnownClient>> ListAllDevices(ScpClient client)
        {
            using var clientListStream = client.OpenFileStream(AllDevicesFile);

            var activeClientsRepresentation = await clientListStream.DeserializeAsync<AllClientsRepresentation>();

            return activeClientsRepresentation.Devices;
        }
    }
}
