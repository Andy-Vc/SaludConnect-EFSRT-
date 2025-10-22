namespace Models
{
    public class Consultories
    {
        public int idConsultories { get; set; }
        public Specialty Specialty { get; set; }
        public string numberConsultories { get; set; }
        public int FloorNumber { get; set; }
    }
}