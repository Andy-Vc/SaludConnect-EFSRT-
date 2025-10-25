using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Data.Repository;
using Models.DTO;

namespace Logic
{
    public class ConsultoryBL:IConsultories
    {
        private readonly IConsultories service;

        public ConsultoryBL(IConsultories service)
        {
            this.service = service;
        }

        public Task<int> CreateConsultory(CreateConsultoryDTO consultory)
        {
            return service.CreateConsultory(consultory);
        }

        public Task<bool> DeleteConsultory(int idConsultory)
        {
            return service.DeleteConsultory(idConsultory);
        }

        public Task<ConsultoryDTO> GetConsultoryById(int idConsultory)
        {
            return service.GetConsultoryById(idConsultory);
        }

        public Task<List<ConsultoryDTO>> ListConsultories()
        {
            return service.ListConsultories();
        }

        public Task<bool> UpdateConsultory(UpdateConsultoryDTO consultory)
        {
            return service.UpdateConsultory(consultory);
        }
    }
}
