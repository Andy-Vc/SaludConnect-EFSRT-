using Web.Models;
using Web.Models.DTO;

namespace Web.Services.Interface
{
    public interface IMedicalRecord
    {
        Task<ResultResponse<int>> RegisterRecordWithServicesAsync(MedicalRecord record);
    }
}
