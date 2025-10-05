using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserBL serviceBL;

        public UserController(UserBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        [HttpGet("total-doctors")]
        public async Task<IActionResult> totalDoctors()
        {
            var count = await Task.Run(() => serviceBL.totalDoctors());
            return Ok(new { countDoctors = count });
        }
    }
}
