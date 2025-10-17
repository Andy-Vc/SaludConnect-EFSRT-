using System.Text.Json;
using Web.Models;
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
    }
}
