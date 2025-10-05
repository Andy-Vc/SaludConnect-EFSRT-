using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly SpecialtyBL serviceBL;

        public SpecialtyController(SpecialtyBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        [HttpGet("total-specialties")]
        public async Task<IActionResult> totalSpecialties()
        {
            var count = await Task.Run(() => serviceBL.totalSpecialties());
            return Ok(new { countSpecialties = count });
        }
    }
}
