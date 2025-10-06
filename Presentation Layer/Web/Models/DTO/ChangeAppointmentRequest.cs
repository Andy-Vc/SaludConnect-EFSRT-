namespace Web.Models.DTO

{
    public class ChangeAppointmentStateRequest
    {
        public int IdAppointment { get; set; }
        public string State { get; set; }
    }
}
