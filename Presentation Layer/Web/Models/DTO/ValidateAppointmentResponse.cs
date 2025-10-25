using System.Text.Json.Serialization;

namespace Web.Models.DTO
{
    public class ValidateAppointmentResponse
    {
        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("idPatient")]
        public int IdPatient { get; set; }

        [JsonPropertyName("idDoctor")]
        public int IdDoctor { get; set; }

        [JsonPropertyName("idSpecialty")]
        public int IdSpecialty { get; set; }

        [JsonPropertyName("dateAppointment")]
        public DateTime DateAppointment { get; set; }
    }
}
