using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Data.Interface
{
    public interface IService
    {
        Task<List<Service>> ListServicesForDoctor(int idDoctor);
        Task<int> minDurationService();
        Task<int> totalServices();
        Task<List<Service>> ListServicesBySpecialty(int idSpecialty);
    }
}
