using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class PatientNextAppointements
    {
        public int idAppointment { get; set; }
        public string? nombresCompletos { get; set; }
        public string? nameSpeciality { get; set; }
        public string?  horaCita { get; set; }
        public string? fechaCita { get; set; }
        public string? state { get; set; }
        public decimal appointmentPrice { get; set; }
        public string? numberConsultories { get; set; }
        public int floor_number { get; set; }
    }
}
