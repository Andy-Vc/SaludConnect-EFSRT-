using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UpcomingAppointments
    {
        public int idAppointments { get; set; }
        public string? fullNames { get; set; }
        public string? specialty { get; set; }
        public string? hourAppointments { get; set; }
        public string? dateAppointments { get; set; }

    }
}
