using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

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

        [HttpGet("CountAppointments")]
        public async Task<IActionResult> CountAppointments(int idPatient) {
            
            var count = await _patientBL.CountAppointments(idPatient);
            return Ok(count);
        }

        [HttpGet("CountAppointmentsAssisted")]
        public async Task<IActionResult> CountAppointmentsAssisted(int idPatient) {
            var count = await _patientBL.CountAppointmentsAssisted(idPatient);
            return Ok(count);
        }

        [HttpGet("CountAppointmentsEarring")]
        public async Task<IActionResult> CountAppointmentsEarring(int idPatient)
        {
            var count = await _patientBL.CountAppointmentsEarring(idPatient);
            return Ok(count);
        }

        [HttpGet("CountAppointmentsCanceled")]
        public async Task<IActionResult> CountAppointmentsCanceled(int idPatient)
        {
            var count = await _patientBL.CountAppointmentsCanceled(idPatient);
            return Ok(count);
        }

        [HttpGet("TotalDoctors")]
        public async Task<IActionResult> TotalDoctors() 
        { 
            var count = await _patientBL.TotalDoctors();
            return Ok(count);
        }


        [HttpGet("UpcomingAppointments")]
        public async Task<IActionResult> UpcomingAppointmentsPatient(int idPatient) {
            List<UpcomingAppointments> listaMostrar = new List<UpcomingAppointments>();

            listaMostrar = await _patientBL.UpcomingAppointmentsPatient(idPatient);
            return Ok(listaMostrar);
        }

        [HttpGet("PatientInformation")]
        public async Task<IActionResult> PatientInformation(int idUser)
        { 
            List<PatientInformation>listMostrar = new List<PatientInformation>();

            listMostrar = await _patientBL.PatientInformation(idUser);
            return Ok(listMostrar);
        }

    }
}
