using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DashboardServer.Services.LanMonitor.Infrastructure
{
    public static class DeserializationExtensions
    {
        static JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        public static async Task<T> DeserializeAsync<T>(this Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, options: SerializerOptions);
        }
    }
}
