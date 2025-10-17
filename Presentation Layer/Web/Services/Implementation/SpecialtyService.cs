using System.Text.Json;
using Web.Models;
using Web.Models.ViewModels.DoctorVM;
using Web.Services.Interface;

namespace Web.Services.Implementation
{
    public class SpecialtyService : ISpecialty
    {
        private readonly HttpClient _httpClient;

        public SpecialtyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Specialty>> ListSpecialties()
        {
            var url = "specialty/list-specialties";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var specialties = JsonSerializer.Deserialize<List<Specialty>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return specialties ?? new List<Specialty>();
            }
            return new List<Specialty>();
        }


        public async Task<int> totalSpecialties()
        {
            var response = await _httpClient.GetAsync($"specialty/total-specialties");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AboutVM>();
                return result?.countSpecialties ?? 0;
            }
            return 0;
        }
    }
}
