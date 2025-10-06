using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Data.Interface
{
    public interface IAuthorization
    {
        Task<ResultResponse<User>> Login(string email, string password);
        Task<ResultResponse<int>> RegisterPatient(PatientDTO patient);
    }
}
