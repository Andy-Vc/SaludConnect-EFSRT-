using Web.Models.ViewModels.PatientVM;

namespace Web.Models.ViewModels.PatientWM
{
    public class BookingConfirmViewModel
    {
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }
        public int IdSpecialty { get; set; }
        public DateTime DateAppointment { get; set; }
        public DoctorCard DoctorInfo { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

    }
}
