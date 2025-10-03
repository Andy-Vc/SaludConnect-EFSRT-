using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
