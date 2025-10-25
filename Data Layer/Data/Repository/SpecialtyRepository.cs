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
    public class SpecialtyRepository : ISpecialty
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public SpecialtyRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<List<Specialty>> ListSpecialitiesWithDescription()
        {
            {
                var list = new List<Specialty>();

                try
                {
                    using (var conexion = new SqlConnection(stringConexion))
                    {
                        await conexion.OpenAsync();

                        using (var cmd = new SqlCommand("SP_LIST_SPECIALTIES_WITH_DESCRIPTION", conexion))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (reader != null && reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        list.Add(new Specialty
                                        {
                                            IdSpecialty = reader.GetInt32(reader.GetOrdinal("ID_SPECIALTY")),
                                            NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY")),
                                            Description = reader.GetString(reader.GetOrdinal("DESCRIPTION_SPECIALITY")),
                                            DoctorCount = reader.GetInt32(reader.GetOrdinal("DoctorCount"))
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

        public async Task<List<Specialty>> ListSpecialties()
        {
            var list = new List<Specialty>();

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_LIST_SPECIALTIES", conexion))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    list.Add(new Specialty
                                    {
                                        IdSpecialty = reader.GetInt32(reader.GetOrdinal("ID_SPECIALTY")),
                                        NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY")),
                                        Description = reader.GetString(reader.GetOrdinal("DESCRIPTION_SPECIALITY"))
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

        public async Task<int> totalSpecialties()
        {
            int count = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_TOTAL_SPECIALTIES", conexion))
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

        public async Task<List<SpecialtyDTO>> GetAllSpecialties()
        {
            var list = new List<SpecialtyDTO>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_ALL_SPECIALTIES", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new SpecialtyDTO
                                {
                                    IdSpecialty = Convert.ToInt32(reader["ID_SPECIALTY"]),
                                    NameSpecialty = reader["NAME_SPECIALTY"].ToString(),
                                    DescriptionSpeciality = reader["DESCRIPTION_SPECIALITY"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener especialidades: {ex.Message}");
            }
            return list;
        }
    }
}
