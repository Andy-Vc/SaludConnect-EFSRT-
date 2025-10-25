namespace Web.Models.DTO
{
    public class DoctorFullDTO
    {
        public int IdUser { get; set; }
        public string FirstName { get; set; }
        public string LastNamePat { get; set; }
        public string LastNameMat { get; set; }
        public string Document { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phone { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateRegister { get; set; }
        public string ProfilePicture { get; set; }
        public string NameRole { get; set; }

        // Propiedades calculadas para las vistas
        public string FullName => $"{FirstName} {LastNamePat} {LastNameMat}";
        public string GenderDisplay => Gender == 'M' ? "Masculino" : "Femenino";
    }
}
