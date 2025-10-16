using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Office
    {
        public int IdOffice { get; set; }
        public Specialty IdSpecialty { get; set; }
        public string NroOffice { get; set; }
        public int FloorNumber { get; set; }
    }
}
