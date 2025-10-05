using Web.Models;
using Web.Models.ViewModels.DoctorVM;

namespace Web.Services.Interface
{
    public interface IAppointment
    {
        Task<int> CountAppointmentsTodayByDoctor(int doctorId);
        Task<int> CountPatientsByDoctor(int doctorId);
        Task<int> CountCompletedAppointmentByDoctor(int doctorId);
        Task<int> CountUpcomingAppointmentsByDoctor(int doctorId);
        Task<List<Appointment>> ListAppointmentDateByDoctor(int doctorId, DateTime date);
        Task<List<AppointmentSummaryByDate>> GetAppointmentsSummaryLast7Days(int doctorId);
    }
}
