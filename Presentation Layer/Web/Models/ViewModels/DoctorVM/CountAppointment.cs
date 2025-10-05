namespace Web.Models.ViewModels.DoctorVM
{
    public class CountAppointment
    {
        public int AppointmentsToday { get; set; }
        public int CompletedAppointments { get; set; }
        public int TotalPatients { get; set; }
        public int UpcomingAppointments { get; set; }
    }
}
