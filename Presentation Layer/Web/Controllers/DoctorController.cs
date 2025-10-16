using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Web.Extensions;
using Web.Models;
using Web.Models.ViewModels.DoctorVM;
using Web.Services.Implementation;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeAppointmentState(ChangeAppointmentStateRequest request)
        {
            try
            {
                var result = await appointment.ChangeStateAppointment(request.IdAppointment, request.State);

                if (result.Value)
                {
                    TempData["GoodMessage"] = result.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cambiar el estado: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
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

        public async Task<IActionResult> Profile(int page = 1, int pageSize = 6, string searchPatient = null)
        {
            var userSession = HttpContext.Session.GetObjectFromJson<User>("User");
            if (userSession == null || userSession.Role?.IdRole != 3)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            int doctorId = userSession.IdUser;
            var userResult = await user.GetProfile(doctorId);

            if (userResult == null || userResult.Data == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var appointments = await appointment.ListAppointmentDateByDoctor(doctorId, null);

            if (!string.IsNullOrEmpty(searchPatient))
            {
                appointments = appointments.Where(a =>
                    a.Patient != null &&
                    (
                        (a.Patient.FirstName ?? "").ToLower().Contains(searchPatient.ToLower()) ||
                        (a.Patient.LastNamePat ?? "").ToLower().Contains(searchPatient.ToLower()) ||
                        (a.Patient.LastNameMat ?? "").ToLower().Contains(searchPatient.ToLower()) ||
                        $"{a.Patient.FirstName} {a.Patient.LastNamePat}".ToLower().Contains(searchPatient.ToLower()) ||
                        $"{a.Patient.FirstName} {a.Patient.LastNamePat} {a.Patient.LastNameMat}".ToLower().Contains(searchPatient.ToLower())
                    )
                ).ToList();
            }

            var pagedAppointments = appointments
                .OrderByDescending(a => a.DateAppointment)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalAppointments = appointments.Count;
            int totalPages = (int)Math.Ceiling((double)totalAppointments / pageSize);

            var model = new ProfileVM
            {
                User = userResult.Data,
                Appointments = pagedAppointments,
                CurrentPage = page,
                TotalPages = totalPages
            };
            ViewBag.SearchPatient = searchPatient;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DescargarComprobante(int id)
        {
            var (pdfBytes, fileName) = await appointment.DownloadSingleAppointmentPdf(id);

            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound("No se pudo generar el PDF.");

            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}
