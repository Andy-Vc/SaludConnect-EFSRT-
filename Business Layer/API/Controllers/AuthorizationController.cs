using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthorizationBL serviceBL;

        public AuthorizationController(AuthorizationBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ResultResponse<string>("Email y contraseña son requeridos.", false));
            }

            var result = await serviceBL.Login(request.Email, request.Password);

            if (result.Value)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(result);
            }
        }
    }
}
