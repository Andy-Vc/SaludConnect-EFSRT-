using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Data.Interface
{
    public interface IService
    {
        Task<List<Service>> ListServicesForDoctor(int idDoctor);
        Task<int> minDurationService();
        Task<int> totalServices();
        Task<List<Service>> ListServicesBySpecialty(int idSpecialty);



        Task<List<ServiceDTO>> ListServices();
        Task<ServiceDTO> GetServiceById(int idService);
        Task<int> CreateService(CreateServiceDTO service);
        Task<bool> UpdateService(UpdateServiceDTO service);
        Task<bool> DeleteService(int idService);
    }
}
