using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserAuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
