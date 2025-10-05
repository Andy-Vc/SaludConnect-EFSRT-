using System;
using System.Collections.Generic;
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
    public class AuthorizationRepository : IAuthorization
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public AuthorizationRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<ResultResponse<User>> Login(string email, string password)
        {
            User user = null;

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_LOGIN_USER", conexion))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EMAIL", email);
                        cmd.Parameters.AddWithValue("@PASSWORD", password);

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

                                return new ResultResponse<User>($"¡Inicio de sesión exitoso, {user.FirstName} {user.LastNamePat}!", true, user);
                            }
                            else
                            {
                                return new ResultResponse<User>("Usuario o contraseña incorrectos.", false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultResponse<User>($"Error al hacer login: {ex.Message}", false);
            }
        }

    }
}
