<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
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
=======
﻿namespace Models
{
    public class MedicalRecord
    {
        public int idRecord { get; set; }
        public Appointment Appointment { get; set; }
        public DateTime dateReport { get; set; }
        public string observations { get; set; }
        public string diagnosis { get; set; }
        public string treatment { get; set; }
        public DateTime followUpDate { get; set; }
    }
}
>>>>>>> a3335a8c915b068e181d0dc4ed3c33ee6d81bd6b
