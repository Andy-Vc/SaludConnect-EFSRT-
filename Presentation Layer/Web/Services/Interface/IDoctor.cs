using Web.Models.DTO;
using Web.Models.ViewModels.PatientVM;

namespace Web.Services.Interface
{
    public interface IDoctor
    {
        Task<List<DoctorFullDTO>> ListDoctors();
        Task<DoctorDetailDTO> GetDoctorById(int idDoctor);
        Task<int> CreateDoctor(CreateDoctorDTO doctor);
        Task<bool> UpdateDoctor(int id, UpdateDoctorDTO doctor);
        Task<bool> DeleteDoctor(int idDoctor);
        Task<bool> AddDoctorSpecialty(int idDoctor, DoctorSpecialtyDTO doctorSpecialty);
        Task<List<DoctorCard>> ListDoctorsWithExperience(int idSpeciality);
        Task<DoctorCard> GetDoctorInfo(int idDoctor);
    }
}
