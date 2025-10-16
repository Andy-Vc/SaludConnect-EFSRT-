using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class MedicalRecord
    {
        public int IdRecord { get; set; }
        public int IdAppointment { get; set; }
        public DateTime DateReport { get; set; } 
        public string Observations { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }

        public List<AdditionalService> AdditionalServices { get; set; } = new List<AdditionalService>();
    }

}
