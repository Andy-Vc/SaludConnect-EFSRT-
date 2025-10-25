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



        // =============================================
        // CRUD SERVICIOS
        // =============================================

        public async Task<List<ServiceDTO>> ListServices()
        {
            var list = new List<ServiceDTO>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_LIST_SERVICES", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new ServiceDTO
                                {
                                    IdService = Convert.ToInt32(reader["ID_SERVICE"]),
                                    NameService = reader["NAME_SERVICE"].ToString(),
                                    Description = reader["DESCRIPTION"]?.ToString(),
                                    Price = Convert.ToDecimal(reader["PRICE"]),
                                    DurationMinutes = Convert.ToInt32(reader["DURATION_MINUTES"]),
                                    IdSpecialty = Convert.ToInt32(reader["ID_SPECIALTY"]),
                                    NameSpecialty = reader["NAME_SPECIALTY"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar servicios: {ex.Message}");
            }
            return list;
        }

        public async Task<ServiceDTO> GetServiceById(int idService)
        {
            ServiceDTO service = null;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_SERVICE_BY_ID", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_SERVICE", idService);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                service = new ServiceDTO
                                {
                                    IdService = Convert.ToInt32(reader["ID_SERVICE"]),
                                    NameService = reader["NAME_SERVICE"].ToString(),
                                    Description = reader["DESCRIPTION"]?.ToString(),
                                    Price = Convert.ToDecimal(reader["PRICE"]),
                                    DurationMinutes = Convert.ToInt32(reader["DURATION_MINUTES"]),
                                    IdSpecialty = Convert.ToInt32(reader["ID_SPECIALTY"]),
                                    NameSpecialty = reader["NAME_SPECIALTY"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener servicio por ID: {ex.Message}");
            }
            return service;
        }

        public async Task<int> CreateService(CreateServiceDTO service)
        {
            int idService = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_CREATE_SERVICE", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NAME_SERVICE", service.NameService);
                        cmd.Parameters.AddWithValue("@DESCRIPTION", (object)service.Description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PRICE", service.Price);
                        cmd.Parameters.AddWithValue("@DURATION_MINUTES", service.DurationMinutes);
                        cmd.Parameters.AddWithValue("@ID_SPECIALTY", service.IdSpecialty);

                        var result = await cmd.ExecuteScalarAsync();
                        idService = result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear servicio: {ex.Message}");
            }
            return idService;
        }

        public async Task<bool> UpdateService(UpdateServiceDTO service)
        {
            bool success = false;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_UPDATE_SERVICE", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_SERVICE", service.IdService);
                        cmd.Parameters.AddWithValue("@NAME_SERVICE", service.NameService);
                        cmd.Parameters.AddWithValue("@DESCRIPTION", (object)service.Description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PRICE", service.Price);
                        cmd.Parameters.AddWithValue("@DURATION_MINUTES", service.DurationMinutes);
                        cmd.Parameters.AddWithValue("@ID_SPECIALTY", service.IdSpecialty);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar servicio: {ex.Message}");
            }
            return success;
        }

        public async Task<bool> DeleteService(int idService)
        {
            bool success = false;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_DELETE_SERVICE", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_SERVICE", idService);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar servicio: {ex.Message}");
            }
            return success;
        }

    }
}
