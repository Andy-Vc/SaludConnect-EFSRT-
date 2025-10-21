using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("list-doctors-experience")]
        public async Task<IActionResult> listSpecialties(int idSpeciality)
        {
            var list = await serviceBL.ListDoctorsWithExperience(idSpeciality);
            return Ok(list);
        }

    }
}
