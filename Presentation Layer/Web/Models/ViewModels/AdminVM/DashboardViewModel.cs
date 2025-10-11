namespace Web.Models.ViewModels.AdminVM
{
    public class DashboardViewModel
    {
        public string MensajeBienvenida {  get; set; }

        public int TotalAppointments { get; set; }
        public int TotalPacientes { get; set; }

        public string FullName { get; set; }
        public string Rol { get; set; }
       
    }
}
