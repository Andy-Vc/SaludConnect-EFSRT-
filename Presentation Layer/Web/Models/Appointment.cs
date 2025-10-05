namespace Web.Models

{
    public class Appointment
    {
        public int IdAppointment { get; set; }
        public User Patient { get; set; }
        public User Doctor { get; set; }
        public Service Service { get; set; }
        public DateTime DateAppointment { get; set; }
        public string State { get; set; }
    }
}
