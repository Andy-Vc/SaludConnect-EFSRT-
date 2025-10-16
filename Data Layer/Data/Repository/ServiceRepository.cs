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

namespace Data.Repository
{
    public class ServiceRepository : IService
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public ServiceRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<List<Service>> ListServicesBySpecialty(int idSpecialty)
        {
            var list = new List<Service>();

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_LIST_SERVICES_BY_SPECIALTY", conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdSpecialty", idSpecialty);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    list.Add(ConvertReaderToService(reader));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<Service>();
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        public async Task<List<Service>> ListServicesForDoctor(int idDoctor)
        {
            var list = new List<Service>();

            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_LIST_SERVICES_FOR_DOCTOR", conexion))
                    {
                        cmd.Parameters.AddWithValue("@IDDOCTOR", idDoctor);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    list.Add(ConvertReaderToService(reader));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<Service>();
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        public async Task<int> minDurationService()
        {
            int minutes = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_MIN_SERVICE", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        minutes = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return minutes;
        }

        public async Task<int> totalServices()
        {
            int count = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_TOTAL_SERVICES", conexion))
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

        private Service ConvertReaderToService(SqlDataReader reader)
        {
            return new Service()
            {
                IdService = reader.GetInt32(reader.GetOrdinal("ID_SERVICE")),
                Description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
                DurationMinutes = reader.GetInt32(reader.GetOrdinal("DURATION_MINUTES")),
                NameService = reader.GetString(reader.GetOrdinal("NAME_SERVICE")),
                Price = reader.GetDecimal(reader.GetOrdinal("PRICE")),
                Specialty = new Specialty
                {
                    IdSpecialty = reader.GetInt32(reader.GetOrdinal("ID_SPECIALTY")),
                    NameSpecialty = reader.GetString(reader.GetOrdinal("NAME_SPECIALTY"))
                }
            };
        }

    }
}
