using Models;
using Web.Models.DTO;
using Newtonsoft.Json;
using Web.Services.Interface;

namespace Web.Services.Implementation
{
    public class PatientService : IPatient
    {
        private readonly HttpClient _httpClient;

        public PatientService(HttpClient inyect)
        {
            this._httpClient = inyect;            
        }

     /*   public Task<List<RelationShip>> CompletListOfRelationShips()
        {
            throw new NotImplementedException();
        }
     */
        public async Task<int> CountAppointments(int idPatient)
        {
            var response = await _httpClient.GetAsync($"patient/CountAppointments?idPatient={idPatient}");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(json);
        }

        public async Task<int> CountAppointmentsAssisted(int idPatient)
        {
            var response = await _httpClient.GetAsync($"patient/CountAppointmentsAssisted?idPatient={idPatient}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(json);
        }

        public async Task<int> CountAppointmentsCanceled(int idPatient)
        {
            var response = await _httpClient.GetAsync($"patient/CountAppointmentsCanceled?idPatient={idPatient}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(json);
        }

        public async Task<int> CountAppointmentsEarring(int idPatient)
        {
            var response = await _httpClient.GetAsync($"patient/CountAppointmentsEarring?idPatient={idPatient}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(json);
        }

        public Task<List<PatientInformation>> PatientInformation(int idUser)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalDoctors()
        {
            throw new NotImplementedException();
        }

        public Task<PatientUpdate> UpdateInformationPatient(PatientUpdate user, int idUser)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PatientNextAppointements>> PatientNextAppointement(int idPatient) {
            try 
            {
                var response = await _httpClient.GetAsync($"patient/PatientNextAppointments?idPatient={idPatient}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PatientNextAppointements>>(json);
            } catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR en PatientNextAppointement] {e.Message}");
                return new List<PatientNextAppointements>();
            }
           
        }
    }
}
