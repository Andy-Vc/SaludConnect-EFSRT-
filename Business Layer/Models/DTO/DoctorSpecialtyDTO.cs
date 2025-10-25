using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class DoctorSpecialtyDTO
    {
        public int IdDoctor { get; set; }
        public int IdSpecialty { get; set; }
        public int YearsExperience { get; set; }
        public string Experience { get; set; }
        public string DocLanguages { get; set; }
    }
}
