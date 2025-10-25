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
        Task<DoctorCard> GetDoctorInfo(int idDoctor);



        Task<List<DoctorFullDTO>> ListDoctors();
        Task<DoctorDetailDTO> GetDoctorById(int idDoctor);
        Task<int> CreateDoctor(CreateDoctorDTO doctor);
        Task<bool> UpdateDoctor(UpdateDoctorDTO doctor);
        Task<bool> DeleteDoctor(int idDoctor);
        Task<bool> AddDoctorSpecialty(DoctorSpecialtyDTO doctorSpecialty);

    }
}
