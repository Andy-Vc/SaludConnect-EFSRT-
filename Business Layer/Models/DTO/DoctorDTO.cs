using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Models.DTO
{
    public class DoctorDTO
    {
        public int IdUser { get; set; }
        public string? Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PasswordHash { get; set; }
        [NotMapped]
        public IFormFile? file { get; set; }

    }
}
