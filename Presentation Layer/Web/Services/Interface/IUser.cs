using Web.Models;
using Web.Models.DTO;

namespace Web.Services.Interface
{
    public interface IUser
    {
        Task<int> totalDoctors();
        Task<ResultResponse<User>> GetProfile(int idUser);
        Task<ResultResponse<object>> UpdateProfileDoctor(DoctorDTO doctor);
    }
}
