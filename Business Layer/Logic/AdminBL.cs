using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Models.DTO;

namespace Logic
{
    public class AdminBL: IAdmin
    {
        private readonly IAdmin service;

        public AdminBL(IAdmin service)
        {
            this.service = service;
        }

        public Task<List<AppointmentByStateDTO>> GetAppointmentsByState()
        {
            return service.GetAppointmentsByState();
        }

        public Task<List<MonthlyAppointmentDTO>> GetMonthlyAppointments(int year, int month)
        {
            return service.GetMonthlyAppointments(year, month);
        }

        public Task<int> GetTodayAppointments()
        {
            return service.GetTodayAppointments();
        }

        public Task<List<TopSpecialtyDTO>> GetTopSpecialties(int top = 5)
        {
            return service.GetTopSpecialties(top);
        }

        public Task<int> GetTotalAppointments()
        {
            return service.GetTotalAppointments();
        }

        public Task<int> GetTotalDoctors()
        {
            return service.GetTotalDoctors();
        }

        public Task<int> GetTotalPatients()
        {
            return service.GetTotalPatients();
        }

        public Task<RevenueDTO> GetTotalRevenue()
        {
            return service.GetTotalRevenue();
        }
    }
}
