using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string FirstName { get; set; }
        public string LastNamePat { get; set; }
        public string LastNameMat { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Password{ get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public Role Role { get; set; }
        public DateTime DateRegister { get; set; }
        public string ProfilePicture { get; set; }

        public bool FlgDelete { get; set; }
        public List<DoctorSpecialty> DoctorSpecialties { get; set; }

    }
}
