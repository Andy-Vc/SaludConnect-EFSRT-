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
    public class UserRepository : IUser
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public UserRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<ResultResponse<User>> GetProfile(int idUser)
        {
            User user = null;

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_GET_USER_PROFILE", conexion))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_USER", idUser);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                if (await reader.ReadAsync())
                                {
                                    user = new User
                                    {
                                        IdUser = reader.GetInt32(reader.GetOrdinal("ID_USER")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FIRST_NAME")),
                                        LastNamePat = reader.GetString(reader.GetOrdinal("LAST_NAME_PAT")),
                                        LastNameMat = reader.GetString(reader.GetOrdinal("LAST_NAME_MAT")),
                                        Document = reader.GetString(reader.GetOrdinal("DOCUMENT")),
                                        Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                        Gender = reader.GetString(reader.GetOrdinal("GENDER")),
                                        BirthDate = reader.GetDateTime(reader.GetOrdinal("BIRTHDATE")),
                                        Phone = reader.GetString(reader.GetOrdinal("PHONE")),
                                        Role = new Role
                                        {
                                            IdRole = reader.GetInt32(reader.GetOrdinal("ID_ROLE")),
                                            NameRole = reader.GetString(reader.GetOrdinal("NAME_ROLE"))
                                        }
                                    };
                                }

                                if (user != null && user.Role.IdRole == 3)
                                {
                                    user.DoctorSpecialties = new List<DoctorSpecialty>();

                                    if (await reader.NextResultAsync())
                                    {
                                        while (await reader.ReadAsync())
                                        {
                                            user.DoctorSpecialties.Add(new DoctorSpecialty
                                            {
                                                Specialty = new Specialty
                                                {
                                                    IdSpecialty = reader.GetInt32(reader.GetOrdinal("ID_SPECIALTY")),
                                                    NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY"))
                                                },
                                                YearsExperience = reader.GetInt32(reader.GetOrdinal("YEARS_EXPERIENCE"))
                                            });
                                        }
                                    }
                                }

                                return new ResultResponse<User>("Perfil obtenido con éxito.", true, user);
                            }
                            else
                            {
                                return new ResultResponse<User>("Usuario no encontrado.", false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultResponse<User>($"Error al obtener perfil: {ex.Message}", false);
            }
        }


        public async Task<int> totalDoctors()
        {
            int count = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_TOTAL_DOCTOR", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

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

        public async Task<ResultResponse<object>> UpdateProfileDoctor(DoctorDTO doctor)
        {
            try
            {
                using (var conn = new SqlConnection(stringConexion))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand("SP_UPDATE_PROFILE_DOCTOR", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID_USER", doctor.IdUser);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(doctor.Email) ? (object)DBNull.Value : doctor.Email);
                        cmd.Parameters.AddWithValue("@ProfilePicture", string.IsNullOrEmpty(doctor.ProfilePicture) ? (object)DBNull.Value : doctor.ProfilePicture);
                        cmd.Parameters.AddWithValue("@PasswordHash", string.IsNullOrEmpty(doctor.PasswordHash) ? (object)DBNull.Value : doctor.PasswordHash);

                        await cmd.ExecuteNonQueryAsync();

                        return new ResultResponse<object>
                        {
                            Message = "Perfil actualizado correctamente",
                            Value = true,
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultResponse<object>
                {
                    Message = "Error al actualizar perfil: " + ex.Message,
                    Value = false,
                    Data = null
                };
            }
        }

    }
}
