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
        Task<AppointmentDTO> GetAppointmentForId(int idUser);
        Task<List<AppointmentDTO>> ListAppointmentDateByDoctor(int doctorId, DateTime? date);
        Task<List<AppointmentSummaryByDate>> GetAppointmentsSummaryLast7Days(int doctorId);
        Task<ResultResponse<string>> ChangeStateAppointment(int idAppointment, string state);

        //CORE
        Task<List<AvailableDateAppointment>> SearchAvailableDatesAppointments(int idDoctor, int idEspecialidad);
        Task<List<AvailableTimeSlots>> GetAvailableTimeSlots(int idDoctor, int idSpeciality, DateOnly fecha);

        Task<ValidateAppointmentAvailability> ValidateAppointmentAvailability(int idPatient, int idDoctor, int idSpecialty, DateTime? dateAppointment);
        Task<Appointment> CreateAppointment(Appointment appointment);
        Task<Appointment> GetAppointmentById(int idAppointment);

        Task<Appointment> ChangeStateAppointmentToCancel(int idAppointment);
    
    }
}
