using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Web.Models.DTO;
using Web.Services.Interface;

namespace Web.Services.Implementation
{
    public class DoctorService : IDoctor
    {
        private readonly HttpClient _httpClient;

        public DoctorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<DoctorCard> GetDoctorInfo(int idDoctor)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Doctor/doctor-info/{idDoctor}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DoctorCard>>();
                    return apiResponse?.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetDoctorInfo: {ex.Message}");
                return null;
            }
        }
        public async Task<List<DoctorCard>> ListDoctorsWithExperience(int idSpeciality)
        {
            var url = $"Doctor/list-doctors-experience/{idSpeciality}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<DoctorCard>();
            }

            var content = await response.Content.ReadAsStringAsync();

            var doctors = JsonSerializer.Deserialize<List<DoctorCard>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctors ?? new List<DoctorCard>();
        }
    }
}
