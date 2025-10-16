using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AdditionalService
    {
        public int IdAddService { get; set; }
        public int IdRecord { get; set; }
        public Service Service { get; set; }
        public string State { get; set; }
    }
}
