using System.Text.Json.Serialization;

namespace Web.Models.DTO
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

    }
}
