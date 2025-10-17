using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Data.Interface
{
    public interface IUser
    {
        Task<int> totalDoctors();
        Task<ResultResponse<User>> GetProfile(int idUser);
        Task<ResultResponse<object>> UpdateProfileDoctor(DoctorDTO doctor);
    }
}
