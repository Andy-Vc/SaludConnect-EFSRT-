using Web.Models.DTO;
using Web.Models;


namespace Web.Services.Interface
{
    public interface IPatient
    {
        Task<int> CountAppointments(int idPatient);
        Task<int> CountAppointmentsAssisted(int idPatient);
        Task<int> CountAppointmentsEarring(int idPatient);
        Task<int> CountAppointmentsCanceled(int idPatient);
        Task<int> TotalDoctors();
        Task<PatientInformation> PatientInformation(int idUser);
        Task<List<RelationShip>> CompletListOfRelationShips();
        Task<PatientUpdate> UpdateInformationPatient(PatientUpdate user, Stream? photoStream, string? fileName);
        Task<List<PatientNextAppointements>> PatientNextAppointement(int idPatient);
    }
}
