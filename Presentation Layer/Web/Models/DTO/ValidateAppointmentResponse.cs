namespace Web.Models.DTO
{
    public class ValidateAppointmentResponse
    {
        public bool IsAvailable { get; set; }
        public string ErrorMessage { get; set; }
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }
        public int IdSpecialty { get; set; }
        public DateTime DateAppointment { get; set; }
    }
}
