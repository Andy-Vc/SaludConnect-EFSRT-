using Web.Models;

namespace Web.Services.Interface
{
    public interface IService
    {
        Task<List<Service>> ListServicesForDoctor(int idDoctor);
        Task<int> minDurationService();
        Task<int> totalServices();
    }
}
