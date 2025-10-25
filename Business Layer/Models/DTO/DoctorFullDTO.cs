using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
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
    }
}
