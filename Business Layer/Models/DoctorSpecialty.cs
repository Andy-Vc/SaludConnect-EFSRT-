using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DoctorSpecialty
    {
        public User Doctor { get; set; }
        public Specialty Specialty { get; set; }
        public int YearsExperience { get; set; }
    }
}
