namespace Web.Models.ViewModels.AdminVM
{
    public class DashboardViewModel
    {
        // Información del usuario
        public string FullName { get; set; }
        public string Rol { get; set; }

        // Métricas principales
        public int TotalAppointments { get; set; }
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TodayAppointments { get; set; }

        // Datos financieros
        public decimal TotalRevenue { get; set; }
        public int TotalAppointmentsPaid { get; set; }

        // Datos para gráficos
        public List<AppointmentByState> AppointmentsByState { get; set; }

        public List<TopSpecialty> TopSpecialties { get; set; }
    }
}
