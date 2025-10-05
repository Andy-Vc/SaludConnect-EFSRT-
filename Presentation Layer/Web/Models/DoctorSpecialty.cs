namespace Web.Models

{
    public class DoctorSpecialty
    {
        public User Doctor { get; set; }
        public Specialty Specialty { get; set; }
        public int YearsExperience { get; set; }
    }
}
