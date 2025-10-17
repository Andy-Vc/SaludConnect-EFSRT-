using System.Text.Json;
using Web.Models;
using Web.Models.DTO;
using Web.Models.ViewModels.DoctorVM;
using Web.Services.Interface;

namespace Web.Services.Implementation
{
    public class UserService : IUser
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultResponse<User>> GetProfile(int idUser)
        {
            var response = await _httpClient.GetAsync($"user/profile/{idUser}");

            if (!response.IsSuccessStatusCode)
            {
                return new ResultResponse<User>($"Error: {response.StatusCode}", false);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ResultResponse<User>>(responseContent, options);

            return result ?? new ResultResponse<User>("Error al procesar la respuesta.", false);
        }

        public async Task<int> totalDoctors()
        {
            var response = await _httpClient.GetAsync($"user/total-doctors");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AboutVM>();
                return result?.countDoctors ?? 0;
            }
            return 0;
        }

        public async Task<ResultResponse<object>> UpdateProfileDoctor(DoctorDTO doctor)
        {
            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(doctor.IdUser.ToString()), nameof(doctor.IdUser));

            if (!string.IsNullOrEmpty(doctor.Email))
                formData.Add(new StringContent(doctor.Email), nameof(doctor.Email));

            if (!string.IsNullOrEmpty(doctor.PasswordHash))
                formData.Add(new StringContent(doctor.PasswordHash), nameof(doctor.PasswordHash));


            if (doctor.file != null && doctor.file.Length > 0)
            {
                var fileContent = new StreamContent(doctor.file.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(doctor.file.ContentType);
                formData.Add(fileContent, nameof(doctor.file), doctor.file.FileName);
            }

            var response = await _httpClient.PutAsync("user/update-profile-doctor", formData);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ResultResponse<object>($"Error: {response.StatusCode} - {errorContent}", false);
            }

            var result = await response.Content.ReadFromJsonAsync<ResultResponse<object>>();
            return result ?? new ResultResponse<object>("Error al procesar la respuesta", false);
        }

    }
}
