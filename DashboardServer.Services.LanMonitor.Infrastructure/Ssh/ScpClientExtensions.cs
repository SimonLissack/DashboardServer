using Renci.SshNet;
using System.IO;

namespace DashboardServer.Services.LanMonitor.Infrastructure.Ssh
{
    public static class ScpClientExtensions
    {
        public static Stream OpenFileStream(this ScpClient client, string fileLocation)
        {
            var stream = new MemoryStream();

            if (!client.IsConnected)
            {
                client.Connect();
            }

            client.Download(fileLocation, stream);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static StreamReader OpenFileStreamReader(this ScpClient client, string fileLocation)
        {
            return new StreamReader(client.OpenFileStream(fileLocation));
        }
    }
}
