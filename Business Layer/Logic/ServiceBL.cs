using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Models;
using Models.DTO;

namespace Logic
{
    public class ServiceBL : IService
    {
        private readonly IService service;

        public ServiceBL(IService service)
        {
            this.service = service;
        }

        public Task<int> CreateService(CreateServiceDTO service)
        {
            return this.service.CreateService(service);
        }

        public Task<bool> DeleteService(int idService)
        {
            return service.DeleteService(idService);
        }

        public Task<ServiceDTO> GetServiceById(int idService)
        {
            return service.GetServiceById(idService);
        }

        public Task<List<ServiceDTO>> ListServices()
        {
            return service.ListServices();
        }

        public async Task<List<Service>> ListServicesBySpecialty(int idSpecialty)
        {
            return await service.ListServicesBySpecialty(idSpecialty);
        }

        public async Task<List<Service>> ListServicesForDoctor(int idDoctor)
        {
            return await service.ListServicesForDoctor(idDoctor);
        }

        public async Task<int> minDurationService()
        {
            return await service.minDurationService();
        }

        public async Task<int> totalServices()
        {
            return await service.totalServices();
        }

        public Task<bool> UpdateService(UpdateServiceDTO service)
        {
            return this.service.UpdateService(service);
        }
    }
}
