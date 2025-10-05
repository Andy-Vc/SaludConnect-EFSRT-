using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
