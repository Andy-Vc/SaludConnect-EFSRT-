using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index() 
        {
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

        public IActionResult Record() 
        {
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

        public IActionResult Profile() 
        {
            return View();
        }

        public IActionResult Notifications() 
        {
            return View();
        }
        
    }
}
