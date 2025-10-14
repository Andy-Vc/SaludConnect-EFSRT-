using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EmergencyContact
    {
        public int idEContact { get; set; }
        public string namesContact { get; set; }
        public string lastNamePat { get; set; }
        public string lastNameMat { get; set; }
        public RelationShip relationShip { get; set; }
        public string phoneEmergency { get; set; }

    }
}
