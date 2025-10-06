using System.Text.Json;
using Web.Models;
using Web.Models.DTO;
using Web.Models.ViewModels.DoctorVM;
using Web.Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Services.Implementation
{
    public class AppointmentService : IAppointment
    {
        private readonly HttpClient _httpClient;

        public AppointmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultResponse<string>> ChangeStateAppointment(int idAppointment, string state)
        {
            var requestData = new ChangeAppointmentStateRequest
            { IdAppointment = idAppointment,
                State = state };

            var response = await _httpClient.PostAsJsonAsync("appointment/change-state", requestData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResultResponse<string>>();
                return result;
            }

            return new ResultResponse<string>
            {
                Message = "Error al actualizar el estado",
                Value = false,
                Data = null
            };
        }

        public async Task<int> CountAppointmentsTodayByDoctor(int doctorId)
        {
            var response = await _httpClient.GetAsync($"appointment/count-today/{doctorId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CountAppointment>();
                return result?.AppointmentsToday ?? 0;
            }
            return 0;
        }

        public async Task<int> CountCompletedAppointmentByDoctor(int doctorId)
        {
            var response = await _httpClient.GetAsync($"appointment/count-completed/{doctorId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CountAppointment>();
                return result?.CompletedAppointments ?? 0;
            }
            return 0;
        }

        public async Task<int> CountPatientsByDoctor(int doctorId)
        {
            var response = await _httpClient.GetAsync($"appointment/count-patients/{doctorId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CountAppointment>();
                return result?.TotalPatients ?? 0;
            }
            return 0;
        }

        public async Task<int> CountUpcomingAppointmentsByDoctor(int doctorId)
        {
            var response = await _httpClient.GetAsync($"appointment/count-upcoming/{doctorId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CountAppointment>();
                return result?.UpcomingAppointments ?? 0;
            }
            return 0;
        }

        public async Task<byte[]> DownloadSingleAppointmentPdf(int appointmentId)
        {
            var url = $"appointment/appointment-for-id/{appointmentId}/pdf";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return Array.Empty<byte>();
        }

        public async Task<List<AppointmentSummaryByDate>> GetAppointmentsSummaryLast7Days(int doctorId)
        {
            var url = $"appointment/appointments-for-7-days/{doctorId}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<AppointmentSummaryByDate>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return appointments ?? new List<AppointmentSummaryByDate>();
            }
            return new List<AppointmentSummaryByDate>();
        }

        public async Task<List<Appointment>> ListAppointmentDateByDoctor(int doctorId, DateTime? date)
        {
            string? dateString = date.HasValue ? date.Value.ToString("yyyy-MM-dd") : null;
            var url = $"appointment/appointments-by-doctor/{doctorId}?date={dateString}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return appointments ?? new List<Appointment>();
            }
            return new List<Appointment>();
        }
    }
}
