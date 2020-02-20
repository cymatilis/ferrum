using Ferrum.Core.Extensions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ferrum.Core.Utils
{
    public static class Serialization
    {
        public static async Task<T> DeserializeAsync<T>(this HttpContent jsonHttpContent) where T : class, new()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.AddEnumSerializers();
            var stringContent = await jsonHttpContent.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringContent, options);
        }
    }
}
