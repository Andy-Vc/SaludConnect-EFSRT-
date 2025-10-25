using System.Text.Json;
using Web.Models;
using Web.Models.DTO;
using Web.Models.ViewModels.DoctorVM;
using Web.Models.ViewModels.PatientVM;
using Web.Services.Interface;

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

        public async Task<(byte[] FileBytes, string FileName)> DownloadMedicalRecordPdf(int appointmentId)
        {
            var url = $"appointment/appointment-for-id/{appointmentId}/medical-record-pdf";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();

                var contentDisposition = response.Content.Headers.ContentDisposition;
                string fileName = contentDisposition?.FileNameStar
                    ?? contentDisposition?.FileName?.Trim('"')
                    ?? $"Cita_{appointmentId}.pdf";

                return (fileBytes, fileName);
            }

            return (Array.Empty<byte>(), $"Cita_{appointmentId}.pdf");
        }

        public async Task<(byte[] FileBytes, string FileName)> DownloadSingleAppointmentPdf(int appointmentId)
        {
            var url = $"appointment/appointment-for-id/{appointmentId}/pdf";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();

                var contentDisposition = response.Content.Headers.ContentDisposition;
                string fileName = contentDisposition?.FileNameStar
                    ?? contentDisposition?.FileName?.Trim('"')
                    ?? $"Cita_{appointmentId}.pdf";

                return (fileBytes, fileName);
            }

            return (Array.Empty<byte>(), $"Cita_{appointmentId}.pdf");
        }

        public async Task<AppointmentDTO> GetAppointmentById(int appointmentId)
        {
            if (appointmentId <= 0)
                return null;

            var url = $"appointment/appointment-for-id/{appointmentId}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var appointment = JsonSerializer.Deserialize<AppointmentDTO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return appointment;
            }

            return null;
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

        public async Task<List<AppointmentDTO>> ListAppointmentDateByDoctor(int doctorId, DateTime? date)
        {
            string? dateString = date.HasValue ? date.Value.ToString("yyyy-MM-dd") : null;
            var url = $"appointment/appointments-by-doctor/{doctorId}?date={dateString}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<AppointmentDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return appointments ?? new List<AppointmentDTO>();
            }
            return new List<AppointmentDTO>();
        }

        // CORE

        public async Task<Appointment> ChangeStateAppointmentToCancel(int idAppointment)
        {
            try
            {
                var response = await _httpClient.PutAsync($"Appointment/change-to-cancel/{idAppointment}", null);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<Appointment>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return apiResponse?.Data;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error {response.StatusCode}: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception en ChangeStateAppointmentToCancel: {ex.Message}");
                return null;
            }

        }
        public async Task<Appointment> GetAppointmentByIdBooking(int idAppointment)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Appointment/{idAppointment}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<Appointment>>();

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                else
                {
                    Console.WriteLine($"Error HTTP: {response.StatusCode}");
                }

                return new Appointment();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAppointmentByIdBooking: {ex.Message}");
                return new Appointment();
            }
        }
        public async Task<List<AvailableTimeSlots>> GetAvailableTimeSlots(int idDoctor, int idSpeciality, DateOnly fecha)
        {
            try
            {
                string fechaStr = fecha.ToString("yyyy-MM-dd");
                var response = await _httpClient.GetAsync(
                    $"Appointment/available-slots?idDoctor={idDoctor}&idSpecialty={idSpeciality}&date={fechaStr}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<AvailableTimeSlots>>>();

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                else
                {
                    Console.WriteLine($"Error HTTP en GetAvailableTimeSlots: {response.StatusCode}");
                }

                return new List<AvailableTimeSlots>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAvailableTimeSlots: {ex.Message}");
                return new List<AvailableTimeSlots>();
            }
        }
        public async Task<List<AvailableDateAppointment>> SearchAvailableDatesAppointments(int idDoctor, int idEspecialidad)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"Appointment/available-dates?idDoctor={idDoctor}&idSpecialty={idEspecialidad}");

                if (response.IsSuccessStatusCode)
                {
                    // Deserializar como ApiResponse
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<AvailableDateAppointment>>>();

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                else
                {
                    Console.WriteLine($"Error HTTP: {response.StatusCode}");
                }

                return new List<AvailableDateAppointment>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SearchAvailableDatesAppointments: {ex.Message}");
                return new List<AvailableDateAppointment>();
            }
        }
        public async Task<ValidateAppointmentResponse> ValidateAppointmentAvailability(ValidateAppointmentRequest request)
        {
            try
            {
                Console.WriteLine($"Validating: Patient={request.IdPatient}, Doctor={request.IdDoctor}, Specialty={request.IdSpecialty}, Date={request.DateAppointment}");

                var response = await _httpClient.PostAsJsonAsync("Appointment/validate", request);

                Console.WriteLine($"Validate Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Validate Response: {content}");

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<ValidateAppointmentResponse>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return apiResponse?.Data ?? new ValidateAppointmentResponse { IsAvailable = false, ErrorMessage = "Respuesta inválida" };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Validate Error {response.StatusCode}: {errorContent}");
                    return new ValidateAppointmentResponse { IsAvailable = false, ErrorMessage = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ValidateAppointmentAvailability: {ex.Message}");
                return new ValidateAppointmentResponse { IsAvailable = false, ErrorMessage = "Error de conexión" };
            }
        }

        public async Task<Appointment> CreateAppointment(CreateAppointmentRequest request)
        {
            try
            {
                Console.WriteLine($"Creating: Patient={request.IdPatient}, Doctor={request.IdDoctor}, Specialty={request.IdSpecialty}, Date={request.DateAppointment}");

                var response = await _httpClient.PostAsJsonAsync("Appointment", request);

                Console.WriteLine($"Create Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Create Response: {content}");

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<Appointment>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return apiResponse?.Data;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Create Error {response.StatusCode}: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreateAppointment: {ex.Message}");
                return null;
            }
        }
    }
}
