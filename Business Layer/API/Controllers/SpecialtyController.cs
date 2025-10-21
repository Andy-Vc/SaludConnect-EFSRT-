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

        [HttpGet("list-specialties")]
        public async Task<IActionResult> listSpecialties()
        {
            var list = await serviceBL.ListSpecialties();
            return Ok(list);
        }

        [HttpGet("list-specialties-with-description")]
        public async Task<IActionResult> listSpecialtiesWithDescription()
        {
            var list = await serviceBL.ListSpecialitiesWithDescription();
            return Ok(list);
        }
    }
}
