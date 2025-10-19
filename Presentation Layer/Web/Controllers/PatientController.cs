using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Models;
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

        public async  Task<IActionResult> Record() 
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

            ViewBag.TotalAppointments   = totalApointments;
            ViewBag.ApointmentsAssisted = ApointmentsAssisted;
            ViewBag.ApointmentsCanceled = ApointmentsCanceled;
            ViewBag.ApointmentsEarring  = ApointmentsEarring;


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

        public async Task<IActionResult> Profile() 
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User"); 
            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }
           
            var idUserPatient = user.IdUser;
            var patient = await _patient.PatientInformation(idUserPatient);
            
            return View(patient);
        }

        public IActionResult Notifications() 
        {
            return View();
        }
        
    }
}
