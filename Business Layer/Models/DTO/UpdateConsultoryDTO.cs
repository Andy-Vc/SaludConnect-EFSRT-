using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UpdateConsultoryDTO
    {
        public int IdConsultories { get; set; }
        public string NumberConsultories { get; set; }
        public int FloorNumber { get; set; }
        public int IdSpecialty { get; set; }
    }
}
