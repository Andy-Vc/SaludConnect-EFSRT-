namespace Web.Models.ViewModels.PatientVM
{
    public class SpecialtyViewModel
    {
        public int IdSpecialty { get; set; }
        public string NameSpecialty { get; set; }
        public string Description { get; set; }
        public bool FlgDelete { get; set; }
        public int DoctorCount { get; set; }
    }
}
