

using Web.Models.DTO;

namespace Web.Models.ViewModels.DoctorVM
{
    public class ProfileVM
    {
        public User User { get; set; }
        public List<AppointmentDTO> Appointments { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
