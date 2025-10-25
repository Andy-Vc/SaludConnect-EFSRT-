using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;

namespace Data.Interface
{
    public interface IConsultories
    {
        Task<List<ConsultoryDTO>> ListConsultories();
        Task<ConsultoryDTO> GetConsultoryById(int idConsultory);
        Task<int> CreateConsultory(CreateConsultoryDTO consultory);
        Task<bool> UpdateConsultory(UpdateConsultoryDTO consultory);
        Task<bool> DeleteConsultory(int idConsultory);
    }
}
