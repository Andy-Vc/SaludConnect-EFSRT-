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
