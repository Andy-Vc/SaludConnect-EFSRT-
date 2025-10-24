using Web.Models.ViewModels.PatientVM;

namespace Web.Models.ViewModels.PatientWM
{
    public class BookingDateViewModel
    {
        public int IdDoctor { get; set; }
        public int IdSpecialty { get; set; }
        public DoctorCard DoctorInfo { get; set; }
        public List<AvailableDateAppointment> AvailableDates { get; set; } = new List<AvailableDateAppointment>();
    }
}
