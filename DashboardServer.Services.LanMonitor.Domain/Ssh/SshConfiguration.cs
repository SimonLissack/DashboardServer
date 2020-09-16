namespace DashboardServer.Services.LanMonitor.Domain.Ssh
{
    public class SshConfiguration
    {
        public string User { get; set; }
        public string HostName { get; set; }
        public string Password { get; set; }
        public string RsaKeyLocation { get; set; }
    }
}
