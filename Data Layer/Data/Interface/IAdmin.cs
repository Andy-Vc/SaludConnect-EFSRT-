using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;

namespace Data.Interface
{
    public interface IAdmin
    {
        Task<int> GetTotalAppointments();
        Task<int> GetTotalPatients();
        Task<int> GetTotalDoctors();
        Task<List<AppointmentByStateDTO>> GetAppointmentsByState();
        Task<RevenueDTO> GetTotalRevenue();
        Task<int> GetTodayAppointments();
        Task<List<TopSpecialtyDTO>> GetTopSpecialties(int top = 5);
        Task<List<MonthlyAppointmentDTO>> GetMonthlyAppointments(int year, int month);



      
    }

}

