using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;


namespace Data.Repository
{
    public class DoctorRepository : IDoctor
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public DoctorRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<DoctorCard> GetDoctorInfo(int idDoctor)
        {
            DoctorCard doctor = null;

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_LIST_DOCTOR_INFO", conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdDoctor", idDoctor);
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null && await reader.ReadAsync())
                            {
                                doctor = new DoctorCard
                                {
                                    idDoctor = reader.GetInt32(reader.GetOrdinal("IdDoctor")),
                                    fullNameDoc = reader.GetString(reader.GetOrdinal("FullName")),
                                    speciality = new Specialty()
                                    {
                                        IdSpecialty = reader.GetInt32(reader.GetOrdinal("IdSpecialty")),
                                        NameSpecialty = reader.GetString(reader.GetOrdinal("SpecialtyName"))
                                    },
                                    phone = reader.GetString(reader.GetOrdinal("PHONE")),
                                    email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                    imgProfile = reader.IsDBNull(reader.GetOrdinal("ProfilePicture"))
                                                            ? string.Empty
                                                            : reader.GetString(reader.GetOrdinal("ProfilePicture"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo doctor: {ex.Message}");
            }

            return doctor;
        }

        public async Task<List<DoctorCard>> ListDoctorsWithExperience(int idSpeciality)
        {
            {
                var list = new List<DoctorCard>();

                try
                {
                    using (var conexion = new SqlConnection(stringConexion))
                    {
                        await conexion.OpenAsync();

                        using (var cmd = new SqlCommand("SP_LIST_DOCTORS_BY_SPECIALTIES", conexion))
                        {
                            cmd.Parameters.AddWithValue("@IdSpecialty", idSpeciality);
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (reader != null && reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        list.Add(new DoctorCard
                                        {
                                            idDoctor = reader.GetInt32(reader.GetOrdinal("IdDoctor")),
                                            fullNameDoc = reader.GetString(reader.GetOrdinal("FullName")),
                                            speciality = new Specialty()
                                            {
                                                IdSpecialty = reader.GetInt32(reader.GetOrdinal("ID_SPECIALTY")),
                                                NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY"))
                                            },
                                            phone = reader.GetString(reader.GetOrdinal("PHONE")),
                                            email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                            imgProfile = reader.IsDBNull(reader.GetOrdinal("ProfilePicture"))
                                                            ? string.Empty
                                                            : reader.GetString(reader.GetOrdinal("ProfilePicture")),
                                            yearsExperience = (short)reader.GetInt32(reader.GetOrdinal("YearsExperience")),
                                            description = reader.GetString(reader.GetOrdinal("ExperienceDescription")),
                                            languagues = reader.GetString(reader.GetOrdinal("Languages")),
                                            AvailableToday = (short)reader.GetInt32(reader.GetOrdinal("AvailableToday")),
                                            AvailableTomorrow = (short)reader.GetInt32(reader.GetOrdinal("AvailableTomorrow")),
                                            AvailableThisWeek = (short)reader.GetInt32(reader.GetOrdinal("AvailableThisWeek"))

                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return list;
            }
        }




        // =============================================
        // CRUD DOCTORES
        // =============================================

        public async Task<List<DoctorFullDTO>> ListDoctors()
        {
            var list = new List<DoctorFullDTO>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_LIST_DOCTORS", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new DoctorFullDTO
                                {
                                    IdUser = Convert.ToInt32(reader["ID_USER"]),
                                    FirstName = reader["FIRST_NAME"].ToString(),
                                    LastNamePat = reader["LAST_NAME_PAT"].ToString(),
                                    LastNameMat = reader["LAST_NAME_MAT"].ToString(),
                                    Document = reader["DOCUMENT"].ToString(),
                                    Birthdate = Convert.ToDateTime(reader["BIRTHDATE"]),
                                    Phone = reader["PHONE"].ToString(),
                                    Gender = Convert.ToChar(reader["GENDER"]),
                                    Email = reader["EMAIL"].ToString(),
                                    DateRegister = Convert.ToDateTime(reader["DATE_REGISTER"]),
                                    ProfilePicture = reader["PROFILE_PICTURE"]?.ToString(),
                                    NameRole = reader["NAME_ROLE"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar doctores: {ex.Message}");
            }
            return list;
        }

        public async Task<DoctorDetailDTO> GetDoctorById(int idDoctor)
        {
            DoctorDetailDTO doctor = null;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_DOCTOR_BY_ID", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_DOCTOR", idDoctor);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            // Primera tabla: Información del doctor
                            if (await reader.ReadAsync())
                            {
                                doctor = new DoctorDetailDTO
                                {
                                    IdUser = Convert.ToInt32(reader["ID_USER"]),
                                    FirstName = reader["FIRST_NAME"].ToString(),
                                    LastNamePat = reader["LAST_NAME_PAT"].ToString(),
                                    LastNameMat = reader["LAST_NAME_MAT"].ToString(),
                                    Document = reader["DOCUMENT"].ToString(),
                                    Birthdate = Convert.ToDateTime(reader["BIRTHDATE"]),
                                    Phone = reader["PHONE"].ToString(),
                                    Gender = Convert.ToChar(reader["GENDER"]),
                                    Email = reader["EMAIL"].ToString(),
                                    ProfilePicture = reader["PROFILE_PICTURE"]?.ToString(),
                                    DateRegister = Convert.ToDateTime(reader["DATE_REGISTER"]),
                                    Specialties = new List<DoctorSpecialtyDetailDTO>()
                                };
                            }

                            // Segunda tabla: Especialidades del doctor
                            if (await reader.NextResultAsync() && doctor != null)
                            {
                                while (await reader.ReadAsync())
                                {
                                    doctor.Specialties.Add(new DoctorSpecialtyDetailDTO
                                    {
                                        IdSpecialty = Convert.ToInt32(reader["ID_SPECIALTY"]),
                                        NameSpecialty = reader["NAME_SPECIALTY"].ToString(),
                                        YearsExperience = Convert.ToInt32(reader["YEARS_EXPERIENCE"]),
                                        Experience = reader["EXPERIENCE"].ToString(),
                                        DocLanguages = reader["DOC_LANGUAGES"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener doctor por ID: {ex.Message}");
            }
            return doctor;
        }

        public async Task<int> CreateDoctor(CreateDoctorDTO doctor)
        {
            int idUser = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_CREATE_DOCTOR", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FIRST_NAME", doctor.FirstName);
                        cmd.Parameters.AddWithValue("@LAST_NAME_PAT", doctor.LastNamePat);
                        cmd.Parameters.AddWithValue("@LAST_NAME_MAT", doctor.LastNameMat);
                        cmd.Parameters.AddWithValue("@DOCUMENT", doctor.Document);
                        cmd.Parameters.AddWithValue("@BIRTHDATE", doctor.Birthdate);
                        cmd.Parameters.AddWithValue("@PHONE", doctor.Phone);
                        cmd.Parameters.AddWithValue("@GENDER", doctor.Gender);
                        cmd.Parameters.AddWithValue("@EMAIL", doctor.Email);
                        cmd.Parameters.AddWithValue("@PASSWORD_HASH", doctor.PasswordHash);
                        cmd.Parameters.AddWithValue("@ID_ROLE", 3);
                        cmd.Parameters.AddWithValue("@PROFILE_PICTURE", (object)doctor.ProfilePicture ?? DBNull.Value);

                        var result = await cmd.ExecuteScalarAsync();
                        idUser = result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear doctor: {ex.Message}");
            }
            return idUser;
        }

        public async Task<bool> UpdateDoctor(UpdateDoctorDTO doctor)
        {
            bool success = false;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_UPDATE_DOCTOR", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_USER", doctor.IdUser);
                        cmd.Parameters.AddWithValue("@FIRST_NAME", doctor.FirstName);
                        cmd.Parameters.AddWithValue("@LAST_NAME_PAT", doctor.LastNamePat);
                        cmd.Parameters.AddWithValue("@LAST_NAME_MAT", doctor.LastNameMat);
                        cmd.Parameters.AddWithValue("@PHONE", doctor.Phone);
                        cmd.Parameters.AddWithValue("@EMAIL", doctor.Email);
                        cmd.Parameters.AddWithValue("@PROFILE_PICTURE", (object)doctor.ProfilePicture ?? DBNull.Value);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar doctor: {ex.Message}");
            }
            return success;
        }

        public async Task<bool> DeleteDoctor(int idDoctor)
        {
            bool success = false;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_DELETE_DOCTOR", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_USER", idDoctor);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar doctor: {ex.Message}");
            }
            return success;
        }

        public async Task<bool> AddDoctorSpecialty(DoctorSpecialtyDTO doctorSpecialty)
        {
            bool success = false;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_ADD_DOCTOR_SPECIALTY", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_DOCTOR", doctorSpecialty.IdDoctor);
                        cmd.Parameters.AddWithValue("@ID_SPECIALTY", doctorSpecialty.IdSpecialty);
                        cmd.Parameters.AddWithValue("@YEARS_EXPERIENCE", doctorSpecialty.YearsExperience);
                        cmd.Parameters.AddWithValue("@EXPERIENCE", doctorSpecialty.Experience);
                        cmd.Parameters.AddWithValue("@DOC_LANGUAGES", doctorSpecialty.DocLanguages);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar especialidad al doctor: {ex.Message}");
            }
            return success;
        }

    }
}
