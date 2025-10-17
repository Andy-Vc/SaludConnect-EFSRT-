using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserBL serviceBL;
        private readonly IConfiguration _configuration;
        public UserController(UserBL serviceBL, IConfiguration configuration)
        {
            this.serviceBL = serviceBL;
            _configuration = configuration;
        }

        [HttpGet("total-doctors")]
        public async Task<IActionResult> totalDoctors()
        {
            var count = await Task.Run(() => serviceBL.totalDoctors());
            return Ok(new { countDoctors = count });
        }

        [HttpGet("profile/{idUser}")]
        public async Task<IActionResult> GetProfile(int idUser)
        {
            var result = await serviceBL.GetProfile(idUser);

            if (result.Value)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPut("update-profile-doctor")]
        public async Task<IActionResult> ActualizarPerfilDoctor([FromForm] DoctorDTO doctor)
        {
            try
            {
                if (doctor.file != null && doctor.file.Length > 0)
                {
                    string relativePath = _configuration["UploadsPath"];

                    string rutaUploads = Path.GetFullPath(relativePath, Directory.GetCurrentDirectory());
                    string currentDir = Directory.GetCurrentDirectory();
                    Console.WriteLine("Current directory: " + currentDir);

                    if (!Directory.Exists(rutaUploads))
                        Directory.CreateDirectory(rutaUploads);

                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(doctor.file.FileName);
                    string rutaCompleta = Path.Combine(rutaUploads, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await doctor.file.CopyToAsync(stream);
                    }

                    doctor.ProfilePicture = "/uploads/" + nombreArchivo;
                }

                var result = await serviceBL.UpdateProfileDoctor(doctor);

                if (result.Value)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar perfil: " + ex.Message);
            }
        }

    }
}
