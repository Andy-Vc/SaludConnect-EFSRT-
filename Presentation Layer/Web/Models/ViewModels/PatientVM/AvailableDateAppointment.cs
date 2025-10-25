using System.Text.Json.Serialization;

namespace Web.Models.ViewModels.PatientVM
{
    public class AvailableDateAppointment
    {
        [JsonPropertyName("idSchedule")]
        public int IdSchedule { get; set; }

        [JsonPropertyName("scheduleDate")]
        public DateTime ScheduleDate { get; set; }

        [JsonPropertyName("availableSlots")]
        public int AvailableSlots { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("slotDuration")]
        public int SlotDuration { get; set; }
        public int TotalSlots { get; set; }
        public int OccupiedSlots { get; set; }
        public string DayOfWeek { get; set; }
    }
}
