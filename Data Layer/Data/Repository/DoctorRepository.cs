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
                                            AvailableThisWeek = (short)reader.GetInt32(reader.GetOrdinal("AvailableThisWeek")),
                                            AvailableLabel = reader.GetString(reader.GetOrdinal("AvailableLabel"))
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
    }
}
