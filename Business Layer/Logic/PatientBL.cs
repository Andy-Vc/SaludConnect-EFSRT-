using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Models.DTO;

namespace Logic
{
    public class PatientBL : IPatient
    {
        private readonly IPatient _patient;

        public PatientBL(IPatient service)
        {
            this._patient = service;
        }
        public async Task<int> CountAppointments(int idPatient)
        {
            return await _patient.CountAppointments(idPatient);
        }

        public async Task<int> CountAppointmentsAssisted(int idPatient)
        {
            return await _patient.CountAppointmentsAssisted(idPatient);
        }

        public async Task<int> CountAppointmentsCanceled(int idPatient)
        {
            return await _patient.CountAppointmentsCanceled(idPatient);
        }

        public async Task<int> CountAppointmentsEarring(int idPatient)
        {
            return await _patient.CountAppointmentsEarring(idPatient);
        }

        public async Task<int> TotalDoctors()
        {
            return await _patient.TotalDoctors();
        }

        public async Task<List<UpcomingAppointments>> UpcomingAppointmentsPatient(int idPatient)
        {
          return await _patient.UpcomingAppointmentsPatient(idPatient);
        }


        public async Task<List<PatientInformation>> PatientInformation(int idUser)
        {
            return await _patient.PatientInformation(idUser);
        }
    }
}
