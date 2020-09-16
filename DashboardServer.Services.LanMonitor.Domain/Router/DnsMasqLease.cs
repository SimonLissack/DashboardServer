namespace DashboardServer.Services.LanMonitor.Domain.Router
{
    public record DnsMasqLease(
        int ExpiryTimeInSeconds,
        string MacAddress,
        string IpAddress,
        string HostName,
        string ClientId
    );
}
