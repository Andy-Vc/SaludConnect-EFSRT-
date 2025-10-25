using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class CreateDoctorDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [Display(Name = "Apellido Paterno")]
        public string LastNamePat { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        [Display(Name = "Apellido Materno")]
        public string LastNameMat { get; set; }

        [Required(ErrorMessage = "El documento es obligatorio")]
        [Display(Name = "Documento")]
        public string Document { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [Display(Name = "Género")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        public int IdRole { get; set; }

        [Display(Name = "Foto de Perfil (URL)")]
        public string? ProfilePicture { get; set; }
    }
}
