using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear consultorio: {ex.Message}");
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
