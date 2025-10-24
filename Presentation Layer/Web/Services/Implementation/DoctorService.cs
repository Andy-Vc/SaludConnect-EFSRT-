using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Web.Models.DTO;
using Web.Models.ViewModels.PatientVM;
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
            var url = $"Doctor/doctor-info/{idDoctor}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new DoctorCard();
            }

            var content = await response.Content.ReadAsStringAsync();

            var doctor = JsonSerializer.Deserialize<DoctorCard>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctor ?? new DoctorCard();
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
