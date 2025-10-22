using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class ValidateAppointmentAvailability
    {
        public bool IsAvailable { get; set; }
        public string ErrorMessage { get; set; }  
        public int IdPatient { get; set; }        
        public int IdDoctor { get; set; }         
        public int IdSpecialty { get; set; }     
        public DateTime DateAppointment { get; set; }
    }
}
