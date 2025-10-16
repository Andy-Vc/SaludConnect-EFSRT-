using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AdditionalServices
    {
        public int idAddService { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public Service Service { get; set; }
        public double priceAtTime { get; set; }
        public string state { get; set; }
    }
}
