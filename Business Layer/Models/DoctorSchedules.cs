using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DoctorSchedules
    {
        public int idSchedule { get; set; }
        public User Doctor { get; set; }
        public Consultories Consultories { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int duracionCita { get; set; }
    }
}
