using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Data.Interface
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
