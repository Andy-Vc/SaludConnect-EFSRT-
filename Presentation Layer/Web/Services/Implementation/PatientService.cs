using System.Text.Json;
using Newtonsoft.Json;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interface;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Web.Services.Implementation
{
    public class PatientService : IPatient
    {
        private readonly HttpClient _httpClient;

        public PatientService(HttpClient inyect)
        {
            this._httpClient = inyect;            
        }

       public async Task<List<RelationShip>> CompletListOfRelationShips()
        {
            var response = await _httpClient.GetAsync("patient/CompletListRelation");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<RelationShip>>(json);
        }
     
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

        public async Task<PatientInformation> PatientInformation(int idUser)
        {
            var response = await _httpClient.GetAsync($"patient/PatientInformation?idUser={idUser}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PatientInformation>(json);
        }

        public Task<int> TotalDoctors()
        {
            throw new NotImplementedException();
        }

        public async Task<PatientUpdate> UpdateInformationPatient(PatientUpdate user, Stream? photoStream, string? fileName)
        {
            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(user.firstName ?? ""), "firstName");
            formData.Add(new StringContent(user.lastNamePat ?? ""), "lastNamePat");
            formData.Add(new StringContent(user.lastNameMat ?? ""), "lastNameMat");
            formData.Add(new StringContent(user.document ?? ""), "document");
            formData.Add(new StringContent(user.phone ?? ""), "phone");
            formData.Add(new StringContent(user.email ?? ""), "email");
            formData.Add(new StringContent(user.idUser.ToString()), "idUser");

            if (user.Emergency != null) {
                formData.Add(new StringContent(user.Emergency.idEContact.ToString()), "Emergency.idEContact");
                formData.Add(new StringContent(user.Emergency.namesContact ?? ""), "Emergency.namesContact");
                formData.Add(new StringContent(user.Emergency.lastNamePat ?? ""), "Emergency.lastNamePat");
                formData.Add(new StringContent(user.Emergency.lastNameMat ?? ""), "Emergency.lastNameMat");
                formData.Add(new StringContent(user.Emergency.phoneEmergency ?? ""), "Emergency.phoneEmergency");
               
                if (user.Emergency.relationShip != null)
                {
                    formData.Add(new StringContent(user.Emergency.relationShip.idRelationShip.ToString()), "Emergency.relationShip.idRelationShip");
                    formData.Add(new StringContent(user.Emergency.relationShip.descriptionRelationShip ?? ""), "Emergency.relationShip.descriptionRelationShip");
                }
            }

            if (photoStream != null && fileName!=null) {

                var imageContent = new StreamContent(photoStream);

                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                formData.Add(imageContent, "photo", fileName);
            }


            var response = await _httpClient.PutAsync($"patient/UpdateInformationPatient", formData);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<PatientUpdate>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result!;
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
