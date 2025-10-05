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

        public async Task<List<AppointmentSummaryByDate>> GetAppointmentsSummaryLast7Days(int doctorId)
        {
            return await service.GetAppointmentsSummaryLast7Days(doctorId);
        }

        public async Task<List<Appointment>> ListAppointmentDateByDoctor(int doctorId, DateTime date)
        {
            return await service.ListAppointmentDateByDoctor(doctorId, date);
        }
    }
}
