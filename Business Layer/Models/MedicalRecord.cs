namespace Models
{
    public class MedicalRecord
    {
        public int idRecord { get; set; }
        public Appointment Appointment { get; set; }
        public DateTime dateReport { get; set; }
        public string observations { get; set; }
        public string diagnosis { get; set; }
        public string treatment { get; set; }
        public DateTime followUpDate { get; set; }
    }
}