using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interface
{
    public interface IDoctor
    {
        Task<List<DoctorCard>> ListDoctorsWithExperience(int idSpeciality);
        Task<DoctorCard> GetDoctorInfo();

    }
}
