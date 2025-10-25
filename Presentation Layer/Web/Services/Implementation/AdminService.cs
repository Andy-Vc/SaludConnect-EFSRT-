using System.Net.Http;
using Web.Models.ViewModels.AdminVM;
using Web.Services.Interface;

namespace Web.Services.Implementation
{
    public class AdminService : IAdmin
    {
        private readonly HttpClient _httpClient;

        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetTotalAppointments()
        {
            try
            {
                var response = await _httpClient.GetAsync("Admin/appointments/count-total");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TotalAppointments>();
                    return result?.totalAppointments ?? 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener total de citas: {ex.Message}");
            }
            return 0;
        }

        public async Task<int> GetTotalPatients()
        {
            try
            {
                var response = await _httpClient.GetAsync("Admin/patients/count-total");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TotalPatientsResponse>();
                    return result?.totalPatients ?? 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener total de pacientes: {ex.Message}");
            }
            return 0;
        }

        public async Task<int> GetTotalDoctors()
        {
            try
            {
                var response = await _httpClient.GetAsync("Admin/doctors/count-total");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TotalDoctorsResponse>();
                    return result?.totalDoctors ?? 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener total de doctores: {ex.Message}");
            }
            return 0;
        }

        public async Task<int> GetTodayAppointments()
        {
            try
            {
                var response = await _httpClient.GetAsync("Admin/appointments/today");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TodayAppointmentsResponse>();
                    return result?.todayAppointments ?? 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener citas del día: {ex.Message}");
            }
            return 0;
        }

        public async Task<List<AppointmentByState>> GetAppointmentsByState()
        {
            try
            {
                var response = await _httpClient.GetAsync("Admin/appointments/by-state");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AppointmentsByStateResponse>();
                    return result?.appointmentsByState ?? new List<AppointmentByState>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener citas por estado: {ex.Message}");
            }
            return new List<AppointmentByState>();
        }

        public async Task<RevenueData> GetTotalRevenue()
        {
            try
            {
                var response = await _httpClient.GetAsync("Admin/revenue/total");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RevenueResponse>();
                    return result?.revenue ?? new RevenueData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ingresos: {ex.Message}");
            }
            return new RevenueData();
        }

        public async Task<List<TopSpecialty>> GetTopSpecialties(int top = 5)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Admin/specialties/top?top={top}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TopSpecialtiesResponse>();
                    return result?.topSpecialties ?? new List<TopSpecialty>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener especialidades: {ex.Message}");
            }
            return new List<TopSpecialty>();
        }
    }


}

