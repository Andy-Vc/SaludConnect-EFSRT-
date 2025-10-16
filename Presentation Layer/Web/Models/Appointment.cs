namespace Web.Models

{
    public class Appointment
    {
        public int IdAppointment { get; set; }
        public User Patient { get; set; }
        public User Doctor { get; set; }
        public Specialty Specialty { get; set; }
        public DateTime DateAppointment { get; set; }
        public string State { get; set; }
        public decimal AppointmentPrice { get; set; }
        /* Additional */
        public Office Office { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
    }
}
