using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Data.Interface
{
    public interface IPatient
    {
        Task<int> CountAppointments(int idPatient); 
        Task<int> CountAppointmentsAssisted(int idPatient);
        Task<int> CountAppointmentsEarring(int idPatient);
        Task<int> CountAppointmentsCanceled(int idPatient);
        Task<int> TotalDoctors();
        Task<PatientInformation>PatientInformation(int idUser);
        Task<List<RelationShip>> CompletListOfRelationShips();
        Task<PatientUpdate> UpdateInformationPatient(PatientUpdate user, int idUser);
        Task<List<PatientNextAppointements>> PatientNextAppointement(int idPatient);
        Task<List<RecordAppointmentsDTO>> RecordAppointments(int idPatient); 
       

    }
}                                                                                               
