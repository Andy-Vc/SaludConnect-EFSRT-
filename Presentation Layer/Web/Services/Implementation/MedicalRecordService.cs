using System.Text.Json;
using System.Text;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interface;
using System.Text.Json.Serialization;

namespace Web.Services.Implementation
{
    public class MedicalRecordService : IMedicalRecord
    {
        private readonly HttpClient _httpClient;

        public MedicalRecordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultResponse<int>> RegisterRecordWithServicesAsync(MedicalRecord record)
        {
            var url = "MedicalRecord/RegisterRecordWithService";

            // ⭐ CONFIGURAR SERIALIZACIÓN EN CAMELCASE
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = JsonSerializer.Serialize(record, jsonOptions);
            Console.WriteLine($"📤 JSON enviado a la API:");
            Console.WriteLine(jsonContent);

            var content = new StringContent(
                jsonContent,
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Respuesta exitosa: {responseContent}");

                    var result = JsonSerializer.Deserialize<ResultResponse<int>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result ?? new ResultResponse<int>
                    {
                        Value = false,
                        Message = "Respuesta vacía del servidor"
                    };
                }
                else
                {
                    Console.WriteLine($"❌ Error en API: {response.StatusCode}");
                    Console.WriteLine($"📄 Contenido: {responseContent}");

                    return new ResultResponse<int>
                    {
                        Value = false,
                        Message = $"Error del servidor: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Excepción: {ex.Message}");
                Console.WriteLine($"📚 StackTrace: {ex.StackTrace}");

                return new ResultResponse<int>
                {
                    Value = false,
                    Message = $"Error de conexión: {ex.Message}"
                };
            }
        }
    }


}
