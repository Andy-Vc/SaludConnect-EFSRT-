using Web.Models;
using Web.Models.ViewModels.PatientVM;

namespace Web.Services.Interface
{
    public interface ISpecialty
    {
        Task<int> totalSpecialties();
        Task<List<Specialty>> ListSpecialties();
        Task<List<SpecialtyViewModel>> ListSpecialtiesWithDescription();

    }
}
