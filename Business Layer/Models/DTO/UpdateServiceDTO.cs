using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UpdateServiceDTO
    {
        public int IdService { get; set; }
        public string NameService { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public int IdSpecialty { get; set; }
    }
}
