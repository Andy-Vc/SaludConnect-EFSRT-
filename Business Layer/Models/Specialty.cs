using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Specialty
    {
        public int IdSpecialty { get; set; }
        public string NameSpecialty { get; set; }
        public string Description { get; set; }
        public bool FlgDelete { get; set; }
        public int DoctorCount { get; set; }
    }
}
