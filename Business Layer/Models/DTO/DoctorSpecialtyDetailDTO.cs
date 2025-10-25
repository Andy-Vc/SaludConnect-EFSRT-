using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class DoctorSpecialtyDetailDTO
    {
        public int IdSpecialty { get; set; }
        public string NameSpecialty { get; set; }
        public int YearsExperience { get; set; }
        public string Experience { get; set; }
        public string DocLanguages { get; set; }
    }
}
