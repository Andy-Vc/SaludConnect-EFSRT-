using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Data.Interface
{
    public interface ISpecialty
    {
        Task<int> totalSpecialties();
        Task<List<Specialty>> ListSpecialties();
        Task<List<Specialty>> ListSpecialitiesWithDescription();

        Task<List<SpecialtyDTO>> GetAllSpecialties();
    }
}
