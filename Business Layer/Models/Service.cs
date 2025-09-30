using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Service
    {
        public int IdService { get; set; }
        public string NameService { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public Specialty Specialty { get; set; }
        public bool FlgDelete { get; set; } = false;
    }
}
