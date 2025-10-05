using Web.Models;
using Web.Models.DTO;

namespace Web.Services.Interface
{
    public interface IAuthorization
    {
        Task<ResultResponse<User>> Login(string email, string password);
    }
}
