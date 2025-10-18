using Web.Models;
using Web.Models.DTO;
using Web.Models.ViewModels.DoctorVM;

namespace Web.Services.Interface
{
    public interface IAppointment
    {
        Task<int> CountAppointmentsTodayByDoctor(int doctorId);
        Task<int> CountPatientsByDoctor(int doctorId);
        Task<int> CountCompletedAppointmentByDoctor(int doctorId);
        Task<int> CountUpcomingAppointmentsByDoctor(int doctorId);
        Task<List<AppointmentDTO>> ListAppointmentDateByDoctor(int doctorId, DateTime? date);
        Task<List<AppointmentSummaryByDate>> GetAppointmentsSummaryLast7Days(int doctorId);
        Task<AppointmentDTO> GetAppointmentById(int appointmentId);
        Task<(byte[] FileBytes, string FileName)> DownloadSingleAppointmentPdf(int appointmentId);
        Task<(byte[] FileBytes, string FileName)> DownloadMedicalRecordPdf(int appointmentId);
        Task<ResultResponse<string>> ChangeStateAppointment(int idAppointment, string state);
    }
}
