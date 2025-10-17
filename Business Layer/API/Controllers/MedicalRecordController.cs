using Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private IMedicalRecord medicalRecord;

        public MedicalRecordController(IMedicalRecord medicalRecord)
        {
            this.medicalRecord = medicalRecord;
        }

        [HttpPost("RegisterRecordWithService")]
        public async Task<IActionResult> RegisterMedicalRecord([FromBody] MedicalRecord record)
        {
            try
            {
                if (record == null)
                {
                    return BadRequest(new ResultResponse<int>
                    {
                        Value = false,
                        Message = "Los datos enviados son nulos",
                        Data = 0
                    });
                }

                // Validar ModelState
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    return BadRequest(new ResultResponse<int>
                    {
                        Value = false,
                        Message = $"Datos inválidos: {errors}",
                        Data = 0
                    });
                }

                // Logs para debugging
                Console.WriteLine($"📝 Registrando historia médica para cita: {record.IdAppointment}");
                Console.WriteLine($"📋 Observaciones: {record.Observations}");
                Console.WriteLine($"🔢 Servicios adicionales: {record.AdditionalServices?.Count ?? 0}");

                // Llamar al servicio de base de datos (aquí deberías tener tu lógica real)
                var result = await medicalRecord.RegisterRecordWithServicesAsync(record);

                if (result != null && result.Value)
                {
                    Console.WriteLine($"✅ Historia médica registrada con ID: {result.Data}");
                    return Ok(result);
                }

                return BadRequest(result ?? new ResultResponse<int>
                {
                    Value = false,
                    Message = "No se pudo registrar la historia médica",
                    Data = 0
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar: {ex.Message}");
                Console.WriteLine($"📚 StackTrace: {ex.StackTrace}");

                return BadRequest(new ResultResponse<int>
                {
                    Value = false,
                    Message = $"Error interno: {ex.Message}",
                    Data = 0
                });
            }
        }
    }

}
