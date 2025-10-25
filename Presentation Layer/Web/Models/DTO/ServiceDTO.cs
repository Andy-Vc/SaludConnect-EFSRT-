namespace Web.Models.DTO
{
    public class ServiceDTO
    {
        public int IdService { get; set; }
        public string NameService { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public int IdSpecialty { get; set; }
        public string NameSpecialty { get; set; }
    }
}
