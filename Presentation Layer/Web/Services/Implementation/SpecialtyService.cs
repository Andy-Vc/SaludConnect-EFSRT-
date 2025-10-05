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
