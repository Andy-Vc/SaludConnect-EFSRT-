using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.DTO;

namespace Data.Repository
{
    public class AdminRepository : IAdmin
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public AdminRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<int> GetTotalAppointments()
        {
            int total = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_Total_Appointments", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        total = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el total de citas: {ex.Message}");
                total = 0;
            }

            return total;
        }

        public async Task<int> GetTotalPatients()
        {
            int total = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_TOTAL_PATIENTS", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        total = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el total de pacientes: {ex.Message}");
                total = 0;
            }
            return total;
        }

        public async Task<int> GetTotalDoctors()
        {
            int total = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_TOTAL_DOCTORS", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        total = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el total de doctores: {ex.Message}");
                total = 0;
            }
            return total;
        }

        public async Task<List<AppointmentByStateDTO>> GetAppointmentsByState()
        {
            var list = new List<AppointmentByStateDTO>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_APPOINTMENTS_BY_STATE", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new AppointmentByStateDTO
                                {
                                    StateName = reader["STATE_NAME"].ToString(),
                                    Total = Convert.ToInt32(reader["TOTAL"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener citas por estado: {ex.Message}");
            }
            return list;
        }

        public async Task<RevenueDTO> GetTotalRevenue()
        {
            var revenue = new RevenueDTO();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_TOTAL_REVENUE", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                revenue.TotalRevenue = reader["TOTAL_REVENUE"] != DBNull.Value
                                    ? Convert.ToDecimal(reader["TOTAL_REVENUE"])
                                    : 0;
                                revenue.TotalAppointmentsPaid = Convert.ToInt32(reader["TOTAL_APPOINTMENTS_PAID"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ingresos totales: {ex.Message}");
            }
            return revenue;
        }

        public async Task<int> GetTodayAppointments()
        {
            int total = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_TODAY_APPOINTMENTS", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        total = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener citas del día: {ex.Message}");
                total = 0;
            }
            return total;
        }

        public async Task<List<MonthlyAppointmentDTO>> GetMonthlyAppointments(int year, int month)
        {
            var list = new List<MonthlyAppointmentDTO>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_MONTHLY_APPOINTMENTS", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@YEAR", year);
                        cmd.Parameters.AddWithValue("@MONTH", month);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new MonthlyAppointmentDTO
                                {
                                    AppointmentDate = Convert.ToDateTime(reader["APPOINTMENT_DATE"]),
                                    TotalAppointments = Convert.ToInt32(reader["TOTAL_APPOINTMENTS"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener estadísticas mensuales: {ex.Message}");
            }
            return list;
        }

        public async Task<List<TopSpecialtyDTO>> GetTopSpecialties(int top = 5)
        {
            var list = new List<TopSpecialtyDTO>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_TOP_SPECIALTIES", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TOP", top);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new TopSpecialtyDTO
                                {
                                    NameSpecialty = reader["NAME_SPECIALTY"].ToString(),
                                    TotalAppointments = Convert.ToInt32(reader["TOTAL_APPOINTMENTS"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener especialidades más solicitadas: {ex.Message}");
            }
            return list;
        }

       

        

        

    }
}
