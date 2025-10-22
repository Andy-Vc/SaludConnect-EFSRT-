using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<List<AppointmentDTO>> ListAppointmentDateByDoctor(int doctorId, DateTime? date)
        {
            var list = new List<AppointmentDTO>();
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
                list = new List<AppointmentDTO>();

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
        public async Task<AppointmentDTO> GetAppointmentForId(int idAppointment)
        {
            AppointmentDTO appointment = null;

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
                                appointment = ConvertReaderToFullAppointment(reader);
                            }

                            if (appointment?.MedicalRecord != null && await reader.NextResultAsync())
                            {
                                appointment.MedicalRecord.AdditionalServices = new List<AdditionalService>();

                                while (await reader.ReadAsync())
                                {
                                    appointment.MedicalRecord.AdditionalServices.Add(new AdditionalService
                                    {
                                        IdAddService = reader.GetInt32(reader.GetOrdinal("ID_ADD_SERVICE")),
                                        IdRecord = reader.GetInt32(reader.GetOrdinal("ID_RECORD")),
                                        State = reader.GetString(reader.GetOrdinal("STATE")),
                                        Service = new Service
                                        {
                                            IdService = reader.GetInt32(reader.GetOrdinal("ID_SERVICE")),
                                            NameService = reader.GetString(reader.GetOrdinal("NAME_SERVICE")),
                                            Description = reader.IsDBNull(reader.GetOrdinal("DESCRIPTION"))
                                                ? null
                                                : reader.GetString(reader.GetOrdinal("DESCRIPTION")),
                                            Price = reader.GetDecimal(reader.GetOrdinal("PRICE")),
                                            DurationMinutes = reader.GetInt32(reader.GetOrdinal("DURATION_MINUTES")),
                                            Specialty = new Specialty
                                            {
                                                NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY"))
                                            }
                                        }
                                    });
                                }
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
        private AppointmentDTO ConvertReaderToAppointment(SqlDataReader reader)
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

            MedicalRecord medicalRecord = null;
            if (HasColumn("ID_RECORD") && !reader.IsDBNull(reader.GetOrdinal("ID_RECORD")))
            {
                medicalRecord = new MedicalRecord
                {
                    IdRecord = reader.GetInt32(reader.GetOrdinal("ID_RECORD"))
                };
            }

            return new AppointmentDTO()
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
                Specialty = new Specialty
                {
                    NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY")),
                },
                AppointmentPrice = reader.GetDecimal(reader.GetOrdinal("APPOINTMENT_PRICE")),
                MedicalRecord = medicalRecord
            };
        }
        private AppointmentDTO ConvertReaderToFullAppointment(SqlDataReader reader)
        {
            var dateOnly = reader.GetDateTime(reader.GetOrdinal("DateOnly"));
            var timeStr = reader.GetString(reader.GetOrdinal("AppointmentTime"));

            TimeSpan time;
            if (!TimeSpan.TryParse(timeStr, out time))
            {
                time = TimeSpan.Zero;
            }

            var dateAppointment = dateOnly.Date + time;

            var appointment = new AppointmentDTO()
            {
                IdAppointment = reader.GetInt32(reader.GetOrdinal("ID_APPOINTMENT")),
                Patient = new User
                {
                    IdUser = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FIRST_NAME")),
                    LastNamePat = reader.GetString(reader.GetOrdinal("LAST_NAME_PAT")),
                    LastNameMat = reader.GetString(reader.GetOrdinal("LAST_NAME_MAT")),
                    Document = reader.GetString(reader.GetOrdinal("DOCUMENT")),
                    BirthDate = reader.GetDateTime(reader.GetOrdinal("BIRTHDATE")),
                    Phone = reader.GetString(reader.GetOrdinal("PHONE")),
                    Gender = reader.GetString(reader.GetOrdinal("GENDER")),
                    Email = reader.GetString(reader.GetOrdinal("EMAIL"))
                },
                DateAppointment = dateAppointment,
                State = reader.GetString(reader.GetOrdinal("STATE")),
                Specialty = new Specialty
                {
                    IdSpecialty = reader.GetInt32(reader.GetOrdinal("ID_SPECIALTY")),
                    NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY"))
                },
                AppointmentPrice = reader.GetDecimal(reader.GetOrdinal("APPOINTMENT_PRICE")),
                Office = new Office
                {
                    NroOffice = reader.GetString(reader.GetOrdinal("NUMBER_CONSULTORIES")),
                    FloorNumber = reader.GetInt32(reader.GetOrdinal("FLOOR_NUMBER"))
                },
                Doctor = new User
                {
                    IdUser = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    FirstName = reader.GetString(reader.GetOrdinal("DoctorFirstName")),
                    LastNamePat = reader.GetString(reader.GetOrdinal("DoctorLastNamePat")),
                    LastNameMat = reader.GetString(reader.GetOrdinal("DoctorLastNameMat"))
                }
            };

            if (!reader.IsDBNull(reader.GetOrdinal("ID_RECORD")))
            {
                appointment.MedicalRecord = new MedicalRecord
                {
                    IdRecord = reader.GetInt32(reader.GetOrdinal("ID_RECORD")),
                    IdAppointment = appointment.IdAppointment,
                    DateReport = reader.GetDateTime(reader.GetOrdinal("DATE_REPORT")),
                    Observations = reader.IsDBNull(reader.GetOrdinal("OBSERVATIONS"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("OBSERVATIONS")),
                    Diagnosis = reader.GetString(reader.GetOrdinal("DIAGNOSIS")),
                    Treatment = reader.IsDBNull(reader.GetOrdinal("TREATMENT"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("TREATMENT"))
                };
            }

            return appointment;
        }


        // CORE
        public async Task<List<AvailableDateAppointment>> SearchAvailableDatesAppointments(
            int idDoctor,
            int idEspecialidad)
        {
            var availableDates = new List<AvailableDateAppointment>();

            using (var connection = new SqlConnection(stringConexion))
            {
                using (var command = new SqlCommand("SP_SEARCH_AVAILABLE_DATES_APPOINTMENTS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdDoctor", idDoctor);
                    command.Parameters.AddWithValue("@IdSpecialty", idEspecialidad);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            availableDates.Add(new AvailableDateAppointment
                            {
                                IdSchedule = reader.GetInt32(reader.GetOrdinal("IdSchedule")),
                                IdConsultory = reader.GetInt32(reader.GetOrdinal("IdConsultory")),
                                ScheduleDate = reader.GetDateTime(reader.GetOrdinal("ScheduleDate")),
                                StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                                EndTime = reader.GetDateTime(reader.GetOrdinal("EndTime")),
                                SlotDuration = reader.GetInt32(reader.GetOrdinal("SlotDuration")),
                                TotalSlots = reader.GetInt32(reader.GetOrdinal("TotalSlots")),
                                OccupiedSlots = reader.GetInt32(reader.GetOrdinal("OccupiedSlots")),
                                AvailableSlots = reader.GetInt32(reader.GetOrdinal("AvailableSlots")),
                                DayOfWeek = reader.GetString(reader.GetOrdinal("DayOfWeek"))
                            });
                        }
                    }
                }
            }

            return availableDates;
        }

        public async Task<List<AvailableTimeSlots>> GetAvailableTimeSlots(
            int idDoctor,
            int idSpeciality,
            DateOnly fecha)
        {
            var timeSlots = new List<AvailableTimeSlots>();

            using (var connection = new SqlConnection(stringConexion))
            {
                using (var command = new SqlCommand("SP_GET_AVAILABLE_TIME_SLOTS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdDoctor", idDoctor);
                    command.Parameters.AddWithValue("@IdSpecialty", idSpeciality);
                    command.Parameters.AddWithValue("@ScheduleDate", fecha.ToDateTime(TimeOnly.MinValue));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (reader.FieldCount == 1 && reader.GetName(0) == "Message")
                            {
                                // Sin horario disponible
                                break;
                            }

                            timeSlots.Add(new AvailableTimeSlots
                            {
                                SlotTime = reader.GetDateTime(reader.GetOrdinal("SlotTime")),
                                TimeSlot = reader.GetString(reader.GetOrdinal("TimeSlot")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }

            return timeSlots;
        }

        public async Task<ValidateAppointmentAvailability> ValidateAppointmentAvailability(
                int idPatient,
                int idDoctor,
                int idSpecialty,
                DateTime? dateAppointment)
        {
            using (var connection = new SqlConnection(stringConexion))
            {
                using (var command = new SqlCommand("SP_VALIDATE_APPOINTMENT_AVAILABILITY", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdPatient", idPatient);
                    command.Parameters.AddWithValue("@IdDoctor", idDoctor);
                    command.Parameters.AddWithValue("@IdSpecialty", idSpecialty);
                    command.Parameters.AddWithValue("@DateAppointment", dateAppointment ?? (object)DBNull.Value);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ValidateAppointmentAvailability
                            {
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ErrorMessage = reader.GetString(reader.GetOrdinal("ErrorMessage")),
                                IdPatient = reader.GetInt32(reader.GetOrdinal("IdPatient")),
                                IdDoctor = reader.GetInt32(reader.GetOrdinal("IdDoctor")),
                                IdSpecialty = reader.GetInt32(reader.GetOrdinal("IdSpecialty")),
                                DateAppointment = reader.GetDateTime(reader.GetOrdinal("DateAppointment"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            using (var connection = new SqlConnection(stringConexion))
            {
                using (var command = new SqlCommand("SP_CREATE_APPOINTMENT", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdPatient", appointment.Patient.IdUser);
                    command.Parameters.AddWithValue("@IdDoctor", appointment.Doctor.IdUser);
                    command.Parameters.AddWithValue("@IdSpecialty", appointment.Specialty.IdSpecialty);
                    command.Parameters.AddWithValue("@DateAppointment", appointment.DateAppointment);

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        int newAppointmentId = Convert.ToInt32(result);

                        Console.WriteLine($"ID obtenido del proc: {newAppointmentId}");

                        return await GetAppointmentById(newAppointmentId);
                    }

                    return null;
                }
            }
        }
        public async Task<Appointment> GetAppointmentById(int idAppointment)
        {
            Console.WriteLine($"Buscando cita con ID: {idAppointment}");

            using (var connection = new SqlConnection(stringConexion))
            {
                using (var command = new SqlCommand("SP_GET_APPOINTMENT_DETAILS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdAppointment", idAppointment);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            if (reader["ID_APPOINTMENT"] == DBNull.Value)
                            {
                                return null;
                            }

                            return new Appointment
                            {
                                IdAppointment = reader.GetInt32(reader.GetOrdinal("ID_APPOINTMENT")),
                                Patient = new User
                                {
                                    IdUser = reader.GetInt32(reader.GetOrdinal("ID_PATIENT")),
                                    FirstName = reader.GetString(reader.GetOrdinal("PatientFullName"))
                                },
                                Doctor = new User
                                {
                                    IdUser = reader.GetInt32(reader.GetOrdinal("ID_DOCTOR")),
                                    FirstName = reader.GetString(reader.GetOrdinal("DoctorFullName"))
                                },
                                Specialty = new Specialty
                                {
                                    IdSpecialty = reader.GetInt32(reader.GetOrdinal("ID_SPECIALTY")),
                                    NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY"))
                                },
                                DateAppointment = reader.GetDateTime(reader.GetOrdinal("DATE_APPOINTMENT")),
                                Consultory = new Consultories
                                {
                                    idConsultories = reader.GetInt32(reader.GetOrdinal("ID_CONSULTORIES")),
                                    numberConsultories = reader.GetString(reader.GetOrdinal("ConsultoryNumber")),
                                    FloorNumber = reader.GetInt32(reader.GetOrdinal("FloorNumber"))
                                },
                                State = reader.GetString(reader.GetOrdinal("STATE")),
                                AppointmentPrice = reader.GetDecimal(reader.GetOrdinal("APPOINTMENT_PRICE"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Appointment> ChangeStateAppointmentToCancel(int idAppointment)
        {
            if (idAppointment <= 0)
            {
                return null;
            }

            using (var connection = new SqlConnection(stringConexion))
            {
                using (var command = new SqlCommand("SP_CANCEL_APPOINTMENT", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdAppointment", idAppointment);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                int cancelledAppointmentId = Convert.ToInt32(reader.GetValue(0));

                                return await GetAppointmentById(cancelledAppointmentId);
                            }
                        }
                    }
                    return null;
                }
            }
        }
    }
}
