namespace Web.Models.DTO
{
    public class RecordAppointmentsDTO
    {
        public int idAppointment { get; set; }
        public string? nombresDoctor { get; set; }
        public string? gender { get; set; }
        public string? profilePicture { get; set; }
        public int idSpeciality { get; set; }
        public string? nameSpeciality { get; set; }
        public string? horaCita { get; set; }
        public string? fechaCita { get; set; }

        public string? nameService { get; set; }
        public string? descripcion { get; set; }
        public int? duracionMinutes { get; set; }
        public string? consultorio { get; set; }
        public string? state { get; set; }
        public decimal? appointmentPrice { get; set; }
        public string? diagnosis { get; set; }
        public string? observations { get; set; }
        public string? treatment { get; set; }
    }
}
