using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentBL serviceBL;

        public AppointmentController(AppointmentBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }

        [HttpGet("count-today/{doctorId}")]
        public async Task<IActionResult> CountAppointmentsTodayByDoctor(int doctorId)
        {
            var count = await serviceBL.CountAppointmentsTodayByDoctor(doctorId);
            return Ok(new { AppointmentsToday = count });
        }

        [HttpGet("count-completed/{doctorId}")]
        public async Task<IActionResult> CountCompletedAppointmentByDoctor(int doctorId)
        {
            var count = await serviceBL.CountCompletedAppointmentByDoctor(doctorId);
            return Ok(new { CompletedAppointments = count });
        }

        [HttpGet("count-patients/{doctorId}")]
        public async Task<IActionResult> CountPatientsByDoctor(int doctorId)
        {
            var count = await serviceBL.CountPatientsByDoctor(doctorId);
            return Ok(new { TotalPatients = count });
        }

        [HttpGet("count-upcoming/{doctorId}")]
        public async Task<IActionResult> CountUpcomingAppointmentsByDoctor(int doctorId)
        {
            var count = await serviceBL.CountUpcomingAppointmentsByDoctor(doctorId);
            return Ok(new { UpcomingAppointments = count });
        }

        [HttpGet("appointments-by-doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctorAndDate(int doctorId, [FromQuery] DateTime date)
        {
            if (doctorId <= 0)
                return BadRequest("Doctor ID no válido.");

            var appointments = await serviceBL.ListAppointmentDateByDoctor(doctorId, date);

            if (appointments == null || !appointments.Any())
                return NotFound("No se encontraron citas para esta fecha.");

            return Ok(appointments);
        }
        [HttpGet("appointments-for-7-days/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsSummaryLast7Days(int doctorId)
        {
            if (doctorId <= 0)
                return BadRequest("Doctor ID no válido.");

            var appointments = await serviceBL.GetAppointmentsSummaryLast7Days(doctorId);

            if (appointments == null || !appointments.Any())
                return NotFound("No se encontraron citas para esta fecha.");

            return Ok(appointments);
        }
    }
}
