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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<List<Appointment>> ListAppointmentDateByDoctor(int doctorId, DateTime? date)
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
                        cmd.Parameters.AddWithValue("@Date", date.HasValue ? (object)date.Value.Date : DBNull.Value);
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
        public async Task<Appointment> GetAppointmentForId(int idAppointment)
        {
            Appointment appointment = null;

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_GET_APPOINTEMNT_FOR_ID", conexion))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_APPOINTMENT", idAppointment);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                appointment = ConvertReaderToAppointment(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la cita: {ex.Message}");
                appointment = null;
            }

            return appointment;
        }
        public async Task<ResultResponse<string>> ChangeStateAppointment(int idAppointment, string state)
        {
            using (var connection = new SqlConnection(stringConexion))
            {
                using (var command = new SqlCommand("SP_APPOINTMENT_CHANGE_STATE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IDAPPOINTMENT", idAppointment);
                    command.Parameters.AddWithValue("@STATE", state);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        string message = state switch
                        {
                            "A" => "La cita fue marcada como asistida.",
                            "X" => "La cita ha sido cancelada correctamente.",
                            "P" => "La cita está pendiente.",
                            "N" => "La cita fue marcada como no asistió.",
                            _ => "Estado actualizado correctamente."
                        };

                        return new ResultResponse<string>(message, true);
                    }
                    else
                    {
                        return new ResultResponse<string>("No se encontró la cita con el ID proporcionado", false);
                    }
                }
            }
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
            var columns = Enumerable.Range(0, reader.FieldCount)
                                    .Select(reader.GetName)
                                    .ToList();

            bool HasColumn(string name) => columns.Contains(name, StringComparer.OrdinalIgnoreCase);

            return new Appointment()
            {
                IdAppointment = reader.GetInt32(reader.GetOrdinal("ID_APPOINTMENT")),
                Patient = new User
                {
                    IdUser = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FIRST_NAME")),
                    LastNamePat = reader.GetString(reader.GetOrdinal("LAST_NAME_PAT")),
                    LastNameMat = reader.GetString(reader.GetOrdinal("LAST_NAME_MAT")),
                    Document = HasColumn("DOCUMENT") ? reader.GetString(reader.GetOrdinal("DOCUMENT")) : null,
                    BirthDate = HasColumn("BIRTHDATE") ? reader.GetDateTime(reader.GetOrdinal("BIRTHDATE")) : default(DateTime),
                    Phone = HasColumn("PHONE") ? reader.GetString(reader.GetOrdinal("PHONE")) : ""
                },
                DateAppointment = dateAppointment,
                State = reader.GetString(reader.GetOrdinal("STATE")),
                Service = new Service
                {
                    NameService = reader.GetString(reader.GetOrdinal("NAME_SERVICE")),
                    Price = HasColumn("PRICE") ? reader.GetDecimal(reader.GetOrdinal("PRICE")) : 0m
                }
            };
        }
    }
}
