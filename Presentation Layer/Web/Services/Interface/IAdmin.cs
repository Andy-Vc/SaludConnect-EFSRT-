namespace Web.Services.Interface
{
    public interface IAdmin
    {
        Task<int> GetTotalAppointments();
    }
}
