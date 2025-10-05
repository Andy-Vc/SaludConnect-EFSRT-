using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTO;

namespace Data.Repository
{
    public class AppointmentRepository : IAppointment
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public AppointmentRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }
        public async Task<int> CountAppointmentsTodayByDoctor(int doctorId)
        {
            int count = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_APPOINTMENTS_TODAY_FOR_DOCTOR", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IDDOCTOR", doctorId);

                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }
        public async Task<int> CountCompletedAppointmentByDoctor(int doctorId)
        {
            int count = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_TOTAL_APPOINTMENTS_COMPLETED", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IDDOCTOR", doctorId);

                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }
        public async Task<int> CountPatientsByDoctor(int doctorId)
        {
            int count = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_TOTAL_PATIENTS_FOR_DOCTOR", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IDDOCTOR", doctorId);

                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }
        public async Task<int> CountUpcomingAppointmentsByDoctor(int doctorId)
        {
            int count = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_COUNT_UPCOMMING_APPOINTMENTS_FOR_DOCTOR", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IDDOCTOR", doctorId);

                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }
        public async Task<List<Appointment>> ListAppointmentDateByDoctor(int doctorId, DateTime date)
        {
            var list = new List<Appointment>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_LIST_APPOINTMENTS_BY_DATE_FOR_DOCTOR", conexion))
                    {
                        cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                        cmd.Parameters.AddWithValue("@Date", date.Date);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    list.Add(ConvertReaderToAppointment(reader));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<Appointment>();

            }
            return list;
        }
        public async Task<List<AppointmentSummaryByDate>> GetAppointmentsSummaryLast7Days(int doctorId)
        {
            var result = new List<AppointmentSummaryByDate>();

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_GET_APPOINTMENTS_FOR_7_DAYS", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(new AppointmentSummaryByDate
                                {
                                    Date = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    TotalAppointments = reader.GetInt32(reader.GetOrdinal("TotalAppointments"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = new List<AppointmentSummaryByDate>();
            }

            return result;
        }

        private Appointment ConvertReaderToAppointment(SqlDataReader reader)
        {
            var dateOnly = reader.GetDateTime(reader.GetOrdinal("DateOnly")); 
            var timeStr = reader.GetString(reader.GetOrdinal("AppointmentTime")); 

            TimeSpan time;
            if (!TimeSpan.TryParse(timeStr, out time))
            {
                time = TimeSpan.Zero; 
            }

            var dateAppointment = dateOnly.Date + time;

            return new Appointment()
            {
                IdAppointment = reader.GetInt32(reader.GetOrdinal("ID_APPOINTMENT")),
                Patient = new User
                {
                    IdUser = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FIRST_NAME")),
                    LastNamePat = reader.GetString(reader.GetOrdinal("LAST_NAME_PAT")),
                    LastNameMat = reader.GetString(reader.GetOrdinal("LAST_NAME_MAT")),
                },
                DateAppointment = dateAppointment,
                State = reader.GetString(reader.GetOrdinal("STATE")),
                Service = new Service
                {
                    NameService = reader.GetString(reader.GetOrdinal("NAME_SERVICE"))
                }
            };
        }
    }
}
