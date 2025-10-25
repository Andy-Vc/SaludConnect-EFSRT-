
using Web.Models;
using Web.Models.DTO;

namespace Web.Services.Interface
{
    public interface IService
    {
        Task<List<Service>> ListServicesForDoctor(int idDoctor);
        Task<int> minDurationService();
        Task<int> totalServices();
        Task<List<Service>> ListServicesBySpecialty(int idSpecialty);



        Task<List<ServiceDTO>> ListService();
        Task<ServiceDTO> GetServiceById(int id);
        Task<int> CreateService(CreateServiceDTO service);
        Task<bool> UpdateService(int id, UpdateServiceDTO service);
        Task<bool> DeleteService(int id);
    }
}
