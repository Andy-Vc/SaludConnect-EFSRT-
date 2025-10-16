using Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalReportController : ControllerBase
    {
        private IMedicalRecord medicalRecord;

        public MedicalReportController(IMedicalRecord medicalRecord)
        {
            this.medicalRecord = medicalRecord;
        }

        [HttpPost("RegisterRecordWithService")]
        public async Task<IActionResult> RegisterMedicalRecord([FromBody] MedicalRecord record)
        {
            if (record == null)
                return BadRequest("El registro médico es requerido.");

            var result = await medicalRecord.RegisterRecordWithServicesAsync(record);

            if (result.Value)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
