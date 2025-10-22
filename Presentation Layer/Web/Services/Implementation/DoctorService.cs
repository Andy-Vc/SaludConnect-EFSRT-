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
            try
            {
                var response = await _httpClient.GetAsync($"Doctor/list-doctors-experience/{idSpeciality}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<DoctorCard>>>();
                    return apiResponse?.Data ?? new List<DoctorCard>();
                }

                return new List<DoctorCard>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListDoctorsWithExperience: {ex.Message}");
                return new List<DoctorCard>();
            }
        }
    }
}
