using System.Text.Json.Serialization;

namespace Web.Models.ViewModels.PatientVM
{
    public class AvailableTimeSlots
    {
        [JsonPropertyName("slotTime")]
        public DateTime SlotTime { get; set; }

        [JsonPropertyName("timeSlot")]
        public string TimeSlot { get; set; }

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
