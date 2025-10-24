namespace Web.Models.DTO
{
    public class CreateAppointmentRequest
    {
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }
        public int IdSpecialty { get; set; }
        public DateTime DateAppointment { get; set; }
    }
}
