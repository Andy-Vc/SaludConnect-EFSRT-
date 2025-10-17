namespace Web.Models.DTO

{
    public class DoctorDTO
    {
        public int IdUser { get; set; }
        public string? Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PasswordHash { get; set; }
        public IFormFile? file { get; set; }

    }
}
