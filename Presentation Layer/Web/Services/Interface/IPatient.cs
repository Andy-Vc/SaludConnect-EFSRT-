using Web.Models.DTO;


namespace Web.Services.Interface
{
    public interface IPatient
    {
        Task<int> CountAppointments(int idPatient);
        Task<int> CountAppointmentsAssisted(int idPatient);
        Task<int> CountAppointmentsEarring(int idPatient);
        Task<int> CountAppointmentsCanceled(int idPatient);
        Task<int> TotalDoctors();
        Task<List<PatientInformation>> PatientInformation(int idUser);

       // Task<List<RelationShip>> CompletListOfRelationShips();
        Task<PatientUpdate> UpdateInformationPatient(PatientUpdate user, int idUser);
        Task<List<PatientNextAppointements>> PatientNextAppointement(int idPatient);
    }
}
