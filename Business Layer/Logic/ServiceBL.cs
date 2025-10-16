using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Models;

namespace Logic
{
    public class ServiceBL : IService
    {
        private readonly IService service;

        public ServiceBL(IService service)
        {
            this.service = service;
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
    }
}
