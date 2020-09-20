namespace DashboardServer.Services.LanMonitor.Domain.Router
{
    public record RouterDevice(
        string MacAddress,
        string IpAddress,
        string Name,
        string Vendor,
        string ConnectionType,
        int? Rssi
    );
}
