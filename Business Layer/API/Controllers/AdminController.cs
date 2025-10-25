using Data.Interface;
using Data.Repository;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminBL serviceBL;

        public AdminController(AdminBL serviceBL)
        {
            this.serviceBL = serviceBL;
        }
        [HttpGet("appointments/count-total")]
        public async Task<IActionResult> GetTotalAppointments()
        {
            var total = await serviceBL.GetTotalAppointments();
            return Ok(new { totalAppointments = total });
        }


        [HttpGet("patients/count-total")]
        public async Task<IActionResult> GetTotalPatients()
        {
            var total = await serviceBL.GetTotalPatients();
            return Ok(new { totalPatients = total });
        }

        [HttpGet("doctors/count-total")]
        public async Task<IActionResult> GetTotalDoctors()
        {   
            var total = await serviceBL.GetTotalDoctors();
            return Ok(new { totalDoctors = total });
        }

        [HttpGet("appointments/by-state")]
        public async Task<IActionResult> GetAppointmentsByState()
        {
            var data = await serviceBL.GetAppointmentsByState();
            return Ok(new { appointmentsByState = data });
        }

        [HttpGet("revenue/total")]
        public async Task<IActionResult> GetTotalRevenue()
        {   
            var revenue = await serviceBL.GetTotalRevenue();
            return Ok(new { revenue });
        }

        [HttpGet("specialties/top")]
        public async Task<IActionResult> GetTopSpecialties([FromQuery] int top = 5)
        {
            var specialties = await serviceBL.GetTopSpecialties(top);
            return Ok(new { topSpecialties = specialties });
        }


        [HttpGet("appointments/today")]
        public async Task<IActionResult> GetTodayAppointments()
        {
            var total = await serviceBL.GetTodayAppointments();
            return Ok(new { todayAppointments = total });
        }

        [HttpGet("appointments/monthly")]
        public async Task<IActionResult> GetMonthlyAppointments([FromQuery] int year, [FromQuery] int month)
        {
            if (year <= 0 || month < 1 || month > 12)
            {
                return BadRequest(new { message = "Año y mes inválidos" });
            }

            var data = await serviceBL.GetMonthlyAppointments(year, month);
            return Ok(new { monthlyAppointments = data });
        }

  

    }
}
