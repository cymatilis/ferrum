using System.Text.Json;

namespace Ferrum.Gateway.ErrorHandling
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public string AsJsonString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
