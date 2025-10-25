using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorBL serviceBL;

        public DoctorController(DoctorBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        [HttpGet("list-doctors-experience/{idSpeciality}")]
        public async Task<IActionResult> listSpecialties(int idSpeciality)
        {
            var list = await serviceBL.ListDoctorsWithExperience(idSpeciality);
            return Ok(list);
        }

        [HttpGet("doctor-info/{idDoctor}")]
        public async Task<IActionResult> getDoctorById(int idDoctor)
        {
            var list = await serviceBL.GetDoctorInfo(idDoctor);
            return Ok(list);
        }


        // =============================================
        // CRUD DOCTORES
        // =============================================

        [HttpGet("doctors")]
        public async Task<IActionResult> ListDoctors()
        {
            var doctors = await serviceBL.ListDoctors();
            return Ok(new { doctors });
        }

        [HttpGet("doctors/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await serviceBL.GetDoctorById(id);
            if (doctor == null)
            {
                return NotFound(new { message = "Doctor no encontrado" });
            }
            return Ok(new { doctor });
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDTO doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var idDoctor = await serviceBL.CreateDoctor(doctor);
            if (idDoctor > 0)
            {
                return CreatedAtAction(nameof(GetDoctorById), new { id = idDoctor }, new { idDoctor, message = "Doctor creado exitosamente" });
            }
            return BadRequest(new { message = "Error al crear doctor" });
        }

        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorDTO doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.IdUser)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            var success = await serviceBL.UpdateDoctor(doctor);
            if (success)
            {
                return Ok(new { message = "Doctor actualizado exitosamente" });
            }
            return NotFound(new { message = "Doctor no encontrado o no se pudo actualizar" });
        }

        [HttpDelete("doctors/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var success = await serviceBL.DeleteDoctor(id);
            if (success)
            {
                return Ok(new { message = "Doctor eliminado exitosamente" });
            }
            return NotFound(new { message = "Doctor no encontrado" });
        }

        [HttpPost("doctors/{idDoctor}/specialties")]
        public async Task<IActionResult> AddDoctorSpecialty(int idDoctor, [FromBody] DoctorSpecialtyDTO doctorSpecialty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idDoctor != doctorSpecialty.IdDoctor)
            {
                return BadRequest(new { message = "El ID del doctor no coincide" });
            }

            var success = await serviceBL.AddDoctorSpecialty(doctorSpecialty);
            if (success)
            {
                return Ok(new { message = "Especialidad agregada al doctor exitosamente" });
            }
            return BadRequest(new { message = "Error al agregar especialidad al doctor" });
        }



        
    }
}
