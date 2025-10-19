using Models;

namespace Web.Models.DTO
{
    public class PatientUpdate
    {
        public int idUser { get; set; }
        public string? firstName { get; set; }
        public string? lastNamePat { get; set; }
        public string? lastNameMat { get; set; }
        public string? document { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? imageProfile { get; set; }
        public EmergencyContact Emergency { get; set; }
    }
}
