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
            var response = await _httpClient.GetAsync("Admin/count-total");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TotalAppointments>();
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);
                return result?.totalAppointments ?? 0;
            }

            return 0;
        }
    }
}
