using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientBL _patientBL;
        private string? _urlCloudinary;

        public PatientController(PatientBL serviceBL, IConfiguration config)
        {
            this._patientBL = serviceBL;
            this._urlCloudinary = config.GetSection("Cloudinary").GetSection("URL").Value;
        }

        [HttpPut("UpdateInformationPatient")]
        public async Task<IActionResult> UpdateInformationPatient([FromForm] PatientUpdate patient, IFormFile? photo)
        {
            try
            {
                if (photo != null)
                {
                    Cloudinary cloudinary = new Cloudinary(_urlCloudinary);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(photo.FileName, photo.OpenReadStream()),
                        UseFilename = true,
                        Overwrite = true,
                        Folder = "SaludConnect/Patient"

                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    patient.imageProfile = uploadResult.SecureUrl.ToString();
                }
               
                    var patientUpdate = await _patientBL.UpdateInformationPatient(patient, patient.idUser);
                return Ok(patientUpdate);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpGet("PatientInformation")]
        public async Task<IActionResult> PatientInformation(int idUser)
        {
            var patientInformation = new PatientInformation();

            patientInformation = await _patientBL.PatientInformation(idUser);
            return Ok(patientInformation);
        }

        [HttpGet("CompletListRelation")]
        public async Task<IActionResult> CompletListRelationShip() {
            List<RelationShip> listMostrar = new List<RelationShip>();

            listMostrar = await _patientBL.CompletListOfRelationShips();

            return Ok(listMostrar);
        }

        [HttpGet("PatientNextAppointments")]
        public async Task<IActionResult> PatientNextAppointments(int idPatient) 
        { 
            List<PatientNextAppointements>listView = new List<PatientNextAppointements>();

            listView = await _patientBL.PatientNextAppointement(idPatient);
            return Ok(listView);
        }

        [HttpGet("RecordAppointments")]
        public async Task<IActionResult> RecordAppointments(int idPatient) 
        {
            var lista = new List<RecordAppointmentsDTO>();

            lista = await _patientBL.RecordAppointments(idPatient);
            
            return Ok(lista);
        }
    }
}
