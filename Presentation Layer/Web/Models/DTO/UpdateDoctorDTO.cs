using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class UpdateDoctorDTO
    {
        public int IdUser { get; set; }

        
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        
        [Display(Name = "Apellido Paterno")]
        public string LastNamePat { get; set; }

       
        [Display(Name = "Apellido Materno")]
        public string LastNameMat { get; set; }

        
        [Phone(ErrorMessage = "Teléfono inválido")]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Foto de Perfil (URL)")]
        public string ProfilePicture { get; set; }
    }
}
