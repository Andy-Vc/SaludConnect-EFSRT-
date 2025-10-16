using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class DoctorSchedule
    {
        public int IdSchedule { get; set; }
        public User IdDoctor { get; set; }
        public Office IdOffice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationAppointment { get; set; }
    }
}
