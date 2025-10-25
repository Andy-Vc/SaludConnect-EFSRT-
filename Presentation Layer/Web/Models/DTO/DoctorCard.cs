namespace Web.Models.DTO
{
    public class DoctorCard
    {
        public int idDoctor { get; set; }
        public string fullNameDoc { get; set; }
        public Specialty speciality { get; set; }

        public string description { get; set; }

        public string languagues { get; set; }

        public string phone { get; set; }
        public string email { get; set; }

        public string imgProfile { get; set; }

        public short yearsExperience { get; set; }

        public short AvailableToday { get; set; }
        public short AvailableTomorrow { get; set; }
        public short AvailableThisWeek { get; set; }
        public string AvailableLabel { get; set; }
    }
}
