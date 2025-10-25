using Web.Models.DTO;

namespace Web.Services.Interface
{
    public interface IConsultory
    {
        Task<List<ConsultoryDTO>> ListConsultories();
        Task<ConsultoryDTO> GetConsultoryById(int id);
        Task<int> CreateConsultory(CreateConsultoryDTO consultory);
        Task<bool> UpdateConsultory(int id,UpdateConsultoryDTO consultory);
        Task<bool> DeleteConsultory(int id);
    }
}
