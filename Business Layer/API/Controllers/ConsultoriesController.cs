using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultoriesController : ControllerBase
    {
        private readonly ConsultoryBL serviceBL;


        public ConsultoriesController(ConsultoryBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        // =============================================
        // CRUD CONSULTORIOS
        // =============================================

        [HttpGet]
        public async Task<IActionResult> ListConsultories()
        {
            var consultories = await serviceBL.ListConsultories();
            return Ok(new { consultories });
        }

        [HttpGet("consultories/{id}")]
        public async Task<IActionResult> GetConsultoryById(int id)
        {
            var consultory = await serviceBL.GetConsultoryById(id);
            if (consultory == null)
            {
                return NotFound(new { message = "Consultorio no encontrado" });
            }
            return Ok(new { consultory });
        }

        [HttpPost("consultories")]
        public async Task<IActionResult> CreateConsultory([FromBody] CreateConsultoryDTO consultory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var idConsultory = await serviceBL.CreateConsultory(consultory);
            if (idConsultory > 0)
            {
                return CreatedAtAction(nameof(GetConsultoryById), new { id = idConsultory }, new { idConsultory, message = "Consultorio creado exitosamente" });
            }
            return BadRequest(new { message = "Error al crear consultorio" });
        }

        [HttpPut("consultories/{id}")]
        public async Task<IActionResult> UpdateConsultory(int id, [FromBody] UpdateConsultoryDTO consultory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != consultory.IdConsultories)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            var success = await serviceBL.UpdateConsultory(consultory);
            if (success)
            {
                return Ok(new { message = "Consultorio actualizado exitosamente" });
            }
            return NotFound(new { message = "Consultorio no encontrado o no se pudo actualizar" });
        }

        [HttpDelete("consultories/{id}")]
        public async Task<IActionResult> DeleteConsultory(int id)
        {
            var success = await serviceBL.DeleteConsultory(id);
            if (success)
            {
                return Ok(new { message = "Consultorio eliminado exitosamente" });
            }
            return NotFound(new { message = "Consultorio no encontrado" });
        }
    }
}
