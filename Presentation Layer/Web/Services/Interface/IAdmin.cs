using Web.Models.ViewModels.AdminVM;

namespace Web.Services.Interface
{
    public interface IAdmin
    {
        Task<int> GetTotalAppointments();
        Task<int> GetTotalPatients();
        Task<int> GetTotalDoctors();
        Task<int> GetTodayAppointments();
        Task<List<AppointmentByState>> GetAppointmentsByState();
        Task<RevenueData> GetTotalRevenue();
        Task<List<TopSpecialty>> GetTopSpecialties(int top = 5);
    }
}
