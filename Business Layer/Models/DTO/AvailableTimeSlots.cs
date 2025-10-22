using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class AvailableTimeSlots
    {
        public DateTime SlotTime { get; set; }
        public string TimeSlot { get; set; }
        public bool IsAvailable { get; set; }
        public string Status { get; set; }
    }
}
