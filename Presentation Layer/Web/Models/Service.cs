namespace Web.Models
{ 
    public class Service
    {
        public int IdService { get; set; }
        public string NameService { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public Specialty Specialty { get; set; }
        public bool FlgDelete { get; set; } = false;
    }
}
