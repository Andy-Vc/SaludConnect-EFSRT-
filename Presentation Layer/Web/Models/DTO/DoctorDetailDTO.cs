using Models.DTO;

namespace Web.Models.DTO
{
    public class DoctorDetailDTO
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
        public string ProfilePicture { get; set; }
        public DateTime DateRegister { get; set; }
        public List<DoctorSpecialtyDetailDTO> Specialties { get; set; }

        // Propiedades calculadas
        public string FullName => $"{FirstName} {LastNamePat} {LastNameMat}";
        public string GenderDisplay => Gender == 'M' ? "Masculino" : "Femenino";
        public int Age => DateTime.Now.Year - Birthdate.Year;
    }
}
