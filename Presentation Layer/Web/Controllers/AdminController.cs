using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Extensions;
using Web.Models.ViewModels.AdminVM;
using Web.Services.Interface;

namespace Web.Controllers
{
    public class AdminController : Controller
    {

        private IAdmin _admin;
        private IUser _user;

        public AdminController(IAdmin admin, IUser user)
        {
            _admin = admin;
            _user = user;
        }
        public async Task<IActionResult> Dashboard()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");

            if (user == null || user.Role.IdRole != 2) 
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var totalAppointments = await _admin.GetTotalAppointments();

            var viewModel = new DashboardViewModel
            {
                FullName = $"{user.FirstName} {user.LastNamePat}",
                Rol = $"{user.Role.NameRole}",
                TotalAppointments = totalAppointments
            };

            return View(viewModel);
        }
    }
}
