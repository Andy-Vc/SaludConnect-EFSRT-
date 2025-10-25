using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
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
    }
}
