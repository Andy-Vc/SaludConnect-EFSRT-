using Web.Models.DTO;
using Web.Services.Interface;

namespace Web.Services.Implementation
{     public class ConsultoryService : IConsultory
        {
            private readonly HttpClient _httpClient;

            public ConsultoryService(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<List<ConsultoryDTO>> ListConsultories()
            {
                try
                {
                    var response = await _httpClient.GetAsync("Consultories");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<ConsultoriesResponse>();
                        return result?.consultories ?? new List<ConsultoryDTO>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al listar consultorios: {ex.Message}");
                }
                return new List<ConsultoryDTO>();
            }

            public async Task<ConsultoryDTO> GetConsultoryById(int id)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"Consultories/consultories/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<ConsultoryResponse>();
                        return result?.consultory;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener consultorio: {ex.Message}");
                }
                return null;
            }

            public async Task<int> CreateConsultory(CreateConsultoryDTO consultory)
            {
                try
                {
                    var response = await _httpClient.PostAsJsonAsync("Consultories/consultories", consultory);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<CreateConsultoryResponse>();
                        return result?.idConsultory ?? 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear consultorio: {ex.Message}");
                }
                return 0;
            }

            public async Task<bool> UpdateConsultory(int id , UpdateConsultoryDTO consultory)
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync($"Consultories/consultories/{consultory.IdConsultories}", consultory);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar consultorio: {ex.Message}");
                }
                return false;
            }

            public async Task<bool> DeleteConsultory(int id)
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"Consultories/consultories/{id}");
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar consultorio: {ex.Message}");
                }
                return false;
            }
        }
    }
