using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceBL serviceBL;

        public ServiceController(ServiceBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        [HttpGet]
        [Route("service-by-doctor")]
        public async Task<IActionResult> GetServicesByDoctor(int idDoctor)
        {
            var result = await Task.Run(() => serviceBL.ListServicesForDoctor(idDoctor));
            return Ok(result);
        }

        [HttpGet]
        [Route("total-service")]
        public async Task<IActionResult> GetServices()
        {
            var count = await Task.Run(() => serviceBL.totalServices());
            return Ok(new { countServices = count });
        }

        [HttpGet]
        [Route("min-duration-service")]
        public async Task<IActionResult> GetServiceMinDuration()
        {
            var count = await Task.Run(() => serviceBL.minDurationService());
            return Ok(new { minDurationService = count });
        }

        [HttpGet("services-by-specialty")]
        public async Task<IActionResult> GetServicesBySpecialty(int idSpecialty)
        {
            var result = await Task.Run(() => serviceBL.ListServicesBySpecialty(idSpecialty));
            return Ok(result);
        }



        // =============================================
        // CRUD SERVICIOS
        // =============================================

        [HttpGet]
        public async Task<IActionResult> ListServices()
        {

            var services = await serviceBL.ListServices();
            return Ok(new { services });
        }

        [HttpGet("services/{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await serviceBL.GetServiceById(id);
            if (service == null)
            {
                return NotFound(new { message = "Servicio no encontrado" });
            }
            return Ok(new { service });
        }

        [HttpPost("services")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDTO service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var idService = await serviceBL.CreateService(service);
            if (idService > 0)
            {
                return CreatedAtAction(nameof(GetServiceById), new { id = idService }, new { idService, message = "Servicio creado exitosamente" });
            }
            return BadRequest(new { message = "Error al crear servicio" });
        }

        [HttpPut("services/{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceDTO service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.IdService)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            var success = await serviceBL.UpdateService(service);
            if (success)
            {
                return Ok(new { message = "Servicio actualizado exitosamente" });
            }
            return NotFound(new { message = "Servicio no encontrado o no se pudo actualizar" });
        }

        [HttpDelete("services/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var success = await serviceBL.DeleteService(id);
            if (success)
            {
                return Ok(new { message = "Servicio eliminado exitosamente" });
            }
            return NotFound(new { message = "Servicio no encontrado" });
        }
    }
}
