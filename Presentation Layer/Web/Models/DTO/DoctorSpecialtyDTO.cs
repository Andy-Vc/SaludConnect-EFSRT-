using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class DoctorSpecialtyDTO
    {
        public int IdDoctor { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        [Display(Name = "Especialidad")]
        public int IdSpecialty { get; set; }

        [Required(ErrorMessage = "Los años de experiencia son obligatorios")]
        [Range(0, 50, ErrorMessage = "Los años deben estar entre 0 y 50")]
        [Display(Name = "Años de Experiencia")]
        public int YearsExperience { get; set; }

       
        [Display(Name = "Experiencia")]
        public string Experience { get; set; }

        [Display(Name = "Idiomas")]
        public string DocLanguages { get; set; }
    }
}
