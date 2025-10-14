using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientBL _patientBL;

        public PatientController(PatientBL serviceBL)
        {
            this._patientBL = serviceBL;
        }

        [HttpGet("totalAppointments")]
        public async Task<IActionResult> CountAppoitments(int idPatient) {
            
            var count = await _patientBL.CountAppoitments(idPatient);
            return Ok(count);
        }

    }
}
