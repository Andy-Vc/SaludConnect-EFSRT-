using Data.Interface;
using Data.Repository;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdmin _admin;

        public AdminController(IAdmin admin)
        {
            this._admin = admin;
        }

        [HttpGet("count-total")]
        public async Task<IActionResult> GetTotalAppointments()
        {
            var total = await _admin.GetTotalAppointments();
            return Ok(new { totalAppointments = total });
        }

    }
}
