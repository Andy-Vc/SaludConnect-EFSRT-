namespace Web.Models.ViewModels.DoctorVM
{
    public class ProfileVM
    {
        public User User { get; set; }
        public List<Appointment> Appointments { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
