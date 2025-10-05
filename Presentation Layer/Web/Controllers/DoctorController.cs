using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Models;
using Web.Models.ViewModels.DoctorVM;
using Web.Services.Interface;

namespace Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IAppointment appointment;
        private readonly IService service;
        private readonly IUser user;
        private readonly ISpecialty specialty;

        public DoctorController(IAppointment appointment, IService service, IUser user, ISpecialty specialty)
        {
            this.appointment = appointment;
            this.service = service;
            this.user = user;
            this.specialty = specialty;
        }

        #region
        [HttpGet]
        public async Task<IActionResult> GetAppointmentsJson(DateTime date)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");

            if (user == null || user.Role.IdRole != 3)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            int doctorId = user.IdUser;

            var appointments = await appointment.ListAppointmentDateByDoctor(doctorId, date);
            return Json(appointments);
        }
        [HttpGet]
        public async Task<IActionResult> GetAppointmentsChartData()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");

            if (user == null || user.Role.IdRole != 3)
            {
                return Unauthorized();
            }

            int doctorId = user.IdUser;

            var data = await appointment.GetAppointmentsSummaryLast7Days(doctorId);

            var result = new
            {
                labels = data.Select(d => d.Date.ToString("dd MMM")).ToList(),
                values = data.Select(d => d.TotalAppointments).ToList()
            };

            return Json(result);
        }

        #endregion
        public async Task<IActionResult> Dashboard()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");

            if (user == null || user.Role.IdRole != 3) 
            {
                return RedirectToAction("Login", "UserAuth");
            }

            int doctorId = user.IdUser; 

            var countInfo = new CountAppointment
            {
                AppointmentsToday = await appointment.CountAppointmentsTodayByDoctor(doctorId),
                CompletedAppointments = await appointment.CountCompletedAppointmentByDoctor(doctorId),
                TotalPatients = await appointment.CountPatientsByDoctor(doctorId),
                UpcomingAppointments = await appointment.CountUpcomingAppointmentsByDoctor(doctorId)
            };

            var viewModel = new DashboardViewModel
            {
                FullName = $"{user.FirstName} {user.LastNamePat}",
                Rol = $"{user.Role.NameRole}",
                CountInfo = countInfo
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Services(string search, int page = 1, int pageSize = 9)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");

            if (user == null || user.Role.IdRole != 3)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            int doctorId = user.IdUser;
            var listByDoctor = await service.ListServicesForDoctor(doctorId);

            // Filtro por búsqueda (si aplica)
            if (!string.IsNullOrWhiteSpace(search))
            {
                listByDoctor = listByDoctor
                    .Where(s => s.NameService.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            int totalItems = listByDoctor.Count();
            var pagedList = listByDoctor
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["CurrentSearch"] = search;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(pagedList);
        }

        public async Task<IActionResult> About()
        {
            var countInfo = new AboutVM
            {
                countDoctors = await user.totalDoctors(),
                countServices = await service.totalServices(),
                countSpecialties = await specialty.totalSpecialties(),
                minDurationService = await service.minDurationService()
            };
            return View(countInfo);
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
