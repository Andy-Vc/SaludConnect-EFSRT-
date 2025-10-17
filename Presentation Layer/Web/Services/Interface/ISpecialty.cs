using Web.Models;

namespace Web.Services.Interface
{
    public interface ISpecialty
    {
        Task<int> totalSpecialties();
        Task<List<Specialty>> ListSpecialties();
    }
}
