using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UpdateAppointmentRequest
    {
        public int IdAppointment { get; set; }
        public int IdDoctor { get; set; }
        public int IdSpecialty { get; set; }
        public DateTime DateAppointment { get; set; }
        public int IdConsultories { get; set; }
        public string State { get; set; }
    }
}
