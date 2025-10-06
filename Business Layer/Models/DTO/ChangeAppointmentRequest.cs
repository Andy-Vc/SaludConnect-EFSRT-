using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class ChangeAppointmentStateRequest
    {
        public int IdAppointment { get; set; }
        public string State { get; set; }
    }
}
