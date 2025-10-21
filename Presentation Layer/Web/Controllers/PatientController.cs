using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interface;

namespace Web.Controllers
{
    public class PatientController : Controller
    {

        private readonly IPatient _patient;


        public PatientController(IPatient inyec)
        {
            this._patient = inyec;
        }

        public async Task<IActionResult> Index() 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User"); //Obtener al usuario logeado
            if (user == null) 
            {
                return RedirectToAction("Login", "UserAuth");
            } //validacion

            var idUserPatient = user.IdUser;

            var totalApointments    = await _patient.CountAppointments(idUserPatient);
            var ApointmentsAssisted = await _patient.CountAppointmentsAssisted(idUserPatient);
            var ApointmentsCanceled = await _patient.CountAppointmentsCanceled(idUserPatient);
            var ApointmentsEarring  = await _patient.CountAppointmentsEarring(idUserPatient);

            var nextAppointments    = await _patient.PatientNextAppointement(idUserPatient);

            ViewBag.TotalAppointments   = totalApointments;
            ViewBag.ApointmentsAssisted = ApointmentsAssisted;
            ViewBag.ApointmentsCanceled = ApointmentsCanceled;
            ViewBag.ApointmentsEarring  = ApointmentsEarring;
            ViewBag.NextAppointments = nextAppointments;


            return View();
        }
        public IActionResult Booking() 
        {
            return View();
        }
        public IActionResult BookingDoctor()
        {
            return View();
        }
        public IActionResult BookingDate()
        {
            return View();
        }
        public IActionResult BookingSucces()
        {
            return View();
        }

        public async Task<IActionResult> Record() 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User"); //Obtener al usuario logeado
            if (user == null) 
            {
                return RedirectToAction("Login", "UserAuth");
            } //validacion

            var idUserPatient = user.IdUser;

            var totalApointments    = await _patient.CountAppointments(idUserPatient);
            var ApointmentsAssisted = await _patient.CountAppointmentsAssisted(idUserPatient);
            var ApointmentsCanceled = await _patient.CountAppointmentsCanceled(idUserPatient);
            var ApointmentsEarring  = await _patient.CountAppointmentsEarring(idUserPatient);

            var recordAppointments = await _patient.RecordAppointments(idUserPatient);

            ViewBag.TotalAppointments   = totalApointments;
            ViewBag.ApointmentsAssisted = ApointmentsAssisted;
            ViewBag.ApointmentsCanceled = ApointmentsCanceled;
            ViewBag.ApointmentsEarring  = ApointmentsEarring;
            ViewBag.ListAppointments = recordAppointments;

            return View();
        }
        public IActionResult AppointmentsDetails() 
        {
            return View();
        }
        public IActionResult Help() 
        {
            return View();
        }
        
        [HttpGet]
        public async Task<List<RelationShip>> CompletListRelations()
        {
            var Lista = await _patient.CompletListOfRelationShips();
            return Lista;
        }
        public async Task<IActionResult> Profile() 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User"); 
            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            //lista
            var listRelations = await CompletListRelations();
            ViewBag.ListRelations = listRelations;


            var idUserPatient = user.IdUser;
            var patient = await _patient.PatientInformation(idUserPatient);
            
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInformationPatient(PatientUpdate updatePatient, IFormFile? photoFile, string? currentImage) 
        {
            try
            {
                Stream? photoStream = null;
                string? fileName = null;

                if (photoFile != null) { 
                    photoStream = photoFile.OpenReadStream();
                    fileName = photoFile.FileName;
                }

                var updateInformation = await _patient.UpdateInformationPatient(updatePatient, photoStream, fileName);
                if (photoFile == null) {
                    updateInformation.imageProfile = currentImage;
                }

               TempData["SuccesUpdate"] = ($"The patient with ID {updatePatient.idUser} was updated successfully");

                return RedirectToAction("Profile");
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine("Error: " + ex);
                return RedirectToAction("Profile");
            }
        }
        public IActionResult Notifications() 
        {
            return View();
        }
        
    }
}
