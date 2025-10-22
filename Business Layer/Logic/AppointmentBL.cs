using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Models;
using Models.DTO;

namespace Logic
{
    public class AppointmentBL : IAppointment
    {
        private readonly IAppointment service;

        public AppointmentBL(IAppointment service)
        {
            this.service = service;
        }

        public async Task<ResultResponse<string>> ChangeStateAppointment(int IdAppointment, string State)
        {
            return await service.ChangeStateAppointment(IdAppointment, State);
        }

        public async Task<int> CountAppointmentsTodayByDoctor(int doctorId)
        {
            return await service.CountAppointmentsTodayByDoctor(doctorId);
        }

        public async Task<int> CountCompletedAppointmentByDoctor(int doctorId)
        {
            return await service.CountCompletedAppointmentByDoctor(doctorId);
        }

        public async Task<int> CountPatientsByDoctor(int doctorId)
        {
            return await service.CountPatientsByDoctor(doctorId);
        }

        public async Task<int> CountUpcomingAppointmentsByDoctor(int doctorId)
        {
            return await service.CountUpcomingAppointmentsByDoctor(doctorId);
        }

        public async Task<AppointmentDTO> GetAppointmentForId(int idUser)
        {
            return await service.GetAppointmentForId(idUser);
        }

        public async Task<List<AppointmentSummaryByDate>> GetAppointmentsSummaryLast7Days(int doctorId)
        {
            return await service.GetAppointmentsSummaryLast7Days(doctorId);
        }

        public async Task<List<AppointmentDTO>> ListAppointmentDateByDoctor(int doctorId, DateTime? date)
        {
            return await service.ListAppointmentDateByDoctor(doctorId, date);
        }


        //COREEEE
        public async Task<Appointment> GetAppointmentById(int idAppointment)
        {
            return await service.GetAppointmentById(idAppointment);
        }
        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            return await service.CreateAppointment(appointment);
        }

        public async Task<List<AvailableTimeSlots>> GetAvailableTimeSlots(int idDoctor, int idSpeciality, DateOnly fecha)
        {
            return await service.GetAvailableTimeSlots(idDoctor, idSpeciality, fecha);
        }

        public async Task<List<AvailableDateAppointment>> SearchAvailableDatesAppointments(int idDoctor, int idEspecialidad)
        {
            return await service.SearchAvailableDatesAppointments(idDoctor, idEspecialidad);
        }

        public async Task<ValidateAppointmentAvailability> ValidateAppointmentAvailability(int idPatient, int idDoctor, int idSpecialty, DateTime? dateAppointment)
        {
            return await service.ValidateAppointmentAvailability(idPatient, idDoctor, idSpecialty, dateAppointment);
        }

        public async Task<Appointment> ChangeStateAppointmentToCancel(int idAppointment)
        {
            return await service.ChangeStateAppointmentToCancel(idAppointment);
        }
    }
}
