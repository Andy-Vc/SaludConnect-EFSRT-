using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Extensions;
using Web.Models.ViewModels.AdminVM;
using Web.Services.Interface;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin _admin;
        private readonly IUser _user;

        public AdminController(IAdmin admin, IUser user)
        {
            _admin = admin;
            _user = user;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Validar sesión
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            if (user == null || user.Role.IdRole != 2)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            // Obtener todas las métricas en paralelo para mejor rendimiento
            var totalAppointmentsTask = _admin.GetTotalAppointments();
            var totalPatientsTask = _admin.GetTotalPatients();
            var totalDoctorsTask = _admin.GetTotalDoctors();
            var todayAppointmentsTask = _admin.GetTodayAppointments();
            var appointmentsByStateTask = _admin.GetAppointmentsByState();
            var revenueTask = _admin.GetTotalRevenue();
            var topSpecialtiesTask = _admin.GetTopSpecialties(5);

            // Esperar a que todas las tareas terminen
            await Task.WhenAll(
                totalAppointmentsTask,
                totalPatientsTask,
                totalDoctorsTask,
                todayAppointmentsTask,
                appointmentsByStateTask,
                revenueTask,
                topSpecialtiesTask
            );

            // Obtener resultados
            var totalAppointments = await totalAppointmentsTask;
            var totalPatients = await totalPatientsTask;
            var totalDoctors = await totalDoctorsTask;
            var todayAppointments = await todayAppointmentsTask;
            var appointmentsByState = await appointmentsByStateTask;
            var revenue = await revenueTask;
            var topSpecialties = await topSpecialtiesTask;

            // Construir el ViewModel
            var viewModel = new DashboardViewModel
            {
                // Información del usuario
                FullName = $"{user.FirstName} {user.LastNamePat}",
                Rol = user.Role.NameRole,

                // Métricas principales
                TotalAppointments = totalAppointments,
                TotalPatients = totalPatients,
                TotalDoctors = totalDoctors,
                TodayAppointments = todayAppointments,

                // Datos financieros
                TotalRevenue = revenue.TotalRevenue,
                TotalAppointmentsPaid = revenue.TotalAppointmentsPaid,

                // Datos para gráficos
                AppointmentsByState = appointmentsByState,
                TopSpecialties = topSpecialties
            };

            return View(viewModel);
        }

        
    }
}
