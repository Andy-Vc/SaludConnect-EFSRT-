using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class MonthlyAppointmentDTO
    {
        public DateTime AppointmentDate { get; set; }
        public int TotalAppointments { get; set; }
    }
}
