using System.Text.Json;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interface;

namespace Web.Services.Implementation
{
    public class AuthorizationService : IAuthorization
    {
        private readonly HttpClient _httpClient;


        public AuthorizationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultResponse<User>> Login(string email, string password)
        {
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("authorization/login", loginRequest);

            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ResultResponse<User>>(responseContent, options);

            return result ?? new ResultResponse<User>("Error al procesar la respuesta.", false);
        }
    }


}
