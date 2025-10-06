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
    }
}
