using Web.Models.ViewModels.PatientVM;

namespace Web.Services.Interface
{
    public interface IDoctor
    {
        Task<List<DoctorCard>> ListDoctorsWithExperience(int idSpeciality);
        Task<DoctorCard> GetDoctorInfo(int idDoctor);
    }
}
