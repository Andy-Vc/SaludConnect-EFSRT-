
using System.Net;
using System.Text;
using Web.Models.DTO;
using Web.Services.Interface;
using System.Text.Json;
using Web.Models.ViewModels.PatientVM;

namespace Web.Services.Implementation
{
    public class DoctorService : IDoctor

    {
        private readonly HttpClient _httpClient;

        public DoctorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoctorFullDTO>> ListDoctors()
        {
            try
            {
                var response = await _httpClient.GetAsync("Doctor/doctors");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<DoctorsListResponse>();
                    return result?.doctors ?? new List<DoctorFullDTO>();


                }
                Console.WriteLine("Llamando a la API de doctores...");
                Console.WriteLine($"BaseAddress: {_httpClient.BaseAddress}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar doctores: {ex.Message}");
            }
            return new List<DoctorFullDTO>();
        }

        public async Task<DoctorDetailDTO> GetDoctorById(int idDoctor)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Doctor/doctors/{idDoctor}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<DoctorResponse>();
                    return result?.doctor;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener doctor: {ex.Message}");
            }
            return null;
        }

        public async Task<int> CreateDoctor(CreateDoctorDTO doctor)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Doctor/doctors", doctor);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CreateDoctorResponse>();
                    return result?.idDoctor ?? 0;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error HTTP {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al crear doctor: {ex.Message}");
            }
            return 0;
        }



        public async Task<bool> UpdateDoctor(int id, UpdateDoctorDTO doctor)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Doctor/doctors/{id}", doctor);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar doctor: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteDoctor(int idDoctor)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Doctor/doctors/{idDoctor}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar doctor: {ex.Message}");
                return false;
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

            var doctors = System.Text.Json.JsonSerializer.Deserialize<List<DoctorCard>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctors ?? new List<DoctorCard>();
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

            var doctor = System.Text.Json.JsonSerializer.Deserialize<DoctorCard>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctor ?? new DoctorCard();
        }
        public async Task<bool> AddDoctorSpecialty(int idDoctor, DoctorSpecialtyDTO doctorSpecialty)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"Doctor/doctors/{idDoctor}/specialties",
                    doctorSpecialty
                );
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar especialidad: {ex.Message}");
                return false;
            }
        }

       


    }
}
