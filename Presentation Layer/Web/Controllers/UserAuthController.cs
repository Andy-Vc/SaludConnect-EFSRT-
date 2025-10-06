using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Models.DTO;
using Web.Services.Interface;

namespace Web.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly IAuthorization authorization;

        public UserAuthController(IAuthorization authorization)
        {
            this.authorization = authorization;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await authorization.Login(model.Email, model.Password);

            if (result.Value)
            {
                HttpContext.Session.SetObjectAsJson("User", result.Data);

                TempData["GoodMessage"] = result.Message;

                switch (result.Data.Role.IdRole)
                {
                    case 1:
                        return RedirectToAction("Index", "Paciente");
                    case 2:
                        return RedirectToAction("Index", "Admin");
                    case 3:
                        return RedirectToAction("Dashboard", "Doctor");
                    default:
                        return RedirectToAction("Logout", "UserAuth");
                }
            }
            else
            {
                TempData["ErrorMessage"] = result.Message ?? "Credenciales incorrectas.";
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "UserAuth");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(PatientDTO patient)
        {
            var result = await authorization.RegisterPatient(patient);
            if (result.Value)
            {

                TempData["GoodMessage"] = result.Message;
                return RedirectToAction("Login");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message ?? "Credenciales incorrectas.";
                return View(patient);
            }
        }
    }
}
