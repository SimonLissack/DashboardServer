using System.Collections.Generic;
using System.Linq;

namespace DashboardServer.Services.LanMonitor.Infrastructure.Router
{
    /// <summary>
    /// Represents the structure in /tmp/clientlist.json
    /// </summary>
    class ActiveDevicesRepresentation : Dictionary<string, InterfaceDictionary>
    {
        public IEnumerable<ParsedConnectedDevice> GetConnectedDevices() => Values.SelectMany(i => i.ParsedDevices());
    }

    class InterfaceDictionary : Dictionary<string, DeviceList>
    {
        public IEnumerable<ParsedConnectedDevice> ParsedDevices() => this.SelectMany(i => i.Value.Select(d => new ParsedConnectedDevice(
            d.Key,
            d.Value.Ip,
            i.Key,
            ParseOrNull(d.Value.Rssi)
        )));

        static int? ParseOrNull(string input)
        {
            return int.TryParse(input, out var result)
                ? result
                : null;
        }
    }
    class DeviceList : Dictionary<string, Device> { }

    record Device(string Ip, string Rssi);

    public record ParsedConnectedDevice(string MacAddress, string Ip, string Interface, int? Rssi);
}
