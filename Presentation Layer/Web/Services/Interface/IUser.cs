using Web.Models;

namespace Web.Services.Interface
{
    public interface IUser
    {
        Task<int> totalDoctors();
    }
}
