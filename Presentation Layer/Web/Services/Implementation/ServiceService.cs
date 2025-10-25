using System.Text.Json;
using Web.Models;
using Web.Models.DTO;
using Web.Models.ViewModels.DoctorVM;
using Web.Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Services.Implementation
{
    public class ServiceService : IService
    {
        private readonly HttpClient _httpClient;

        public ServiceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Service>> ListServicesBySpecialty(int idSpecialty)
        {
            var url = $"service/services-by-specialty?idSpecialty={idSpecialty}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Service>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return appointments ?? new List<Service>();
            }
            return new List<Service>();
        }

        public async Task<List<Service>> ListServicesForDoctor(int idDoctor)
        {
            var url = $"service/service-by-doctor?idDoctor={idDoctor}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Service>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return appointments ?? new List<Service>();
            }
            return new List<Service>();
        }

        public async Task<int> minDurationService()
        {
            var response = await _httpClient.GetAsync($"service/min-duration-service");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AboutVM>();
                return result?.minDurationService ?? 0;
            }
            return 0;
        }

        public async Task<int> totalServices()
        {
            var response = await _httpClient.GetAsync($"service/total-service");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AboutVM>();
                return result?.countServices ?? 0;
            }
            return 0;
        }


        //CRUDS
        public async Task<List<Models.DTO.ServiceDTO>> ListService()
        {
            try
            {
                var response = await _httpClient.GetAsync("Service");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ServicesResponse>();
                    return result?.services ?? new List<ServiceDTO>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar servicios: {ex.Message}");
            }
            return new List<ServiceDTO>();
        }

        public async Task<ServiceDTO> GetServiceById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Service/services/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
                    return result?.service;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener servicio: {ex.Message}");
            }
            return null;
        }

        public async Task<int> CreateService(CreateServiceDTO service)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Service/services", service);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CreateServiceResponse>();
                    return result?.idService ?? 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear servicio: {ex.Message}");
            }
            return 0;
        }

        public async Task<bool> UpdateService(int id, UpdateServiceDTO service)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Service/services/{service.IdService}", service);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar servicio: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> DeleteService(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Service/services/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar servicio: {ex.Message}");
            }
            return false;
        }


    }
}
