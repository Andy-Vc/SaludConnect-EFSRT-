using Web.Models;

namespace Web.Models.DTO
{
    public class PatientInformation
    {
        public int idUser { get; set; }
        public string? firstName { get; set; }
        public string? lastNamePat { get; set; }
        public string? lastNameMat { get; set; }
        public string? document { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public DateTime dateRegister { get; set; }
        public string imageProfile { get; set; }
        public string? namesContact { get; set; }
        public string lastNamePatContact { get; set; }
        public string lastNameMatContact { get; set; }
        public int idRelationShip { get; set; }
        public string phoneContactEmergency { get; set; }
        public string description_relationShip { get; set; }


    }
}
