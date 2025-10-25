using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.DTO;

namespace Data.Repository
{
    public class ConsultoryRepository : IConsultories
    {

        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public ConsultoryRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<List<ConsultoryDTO>> ListConsultories()
        {
            var list = new List<ConsultoryDTO>();
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_LIST_CONSULTORIES", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new ConsultoryDTO
                                {
                                    IdConsultories = Convert.ToInt32(reader["ID_CONSULTORIES"]),
                                    NumberConsultories = reader["NUMBER_CONSULTORIES"].ToString(),
                                    FloorNumber = Convert.ToInt32(reader["FLOOR_NUMBER"]),
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
                Console.WriteLine($"Error al listar consultorios: {ex.Message}");
            }
            return list;
        }

        public async Task<ConsultoryDTO> GetConsultoryById(int idConsultory)
        {
            ConsultoryDTO consultory = null;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_GET_CONSULTORY_BY_ID", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_CONSULTORIES", idConsultory);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                consultory = new ConsultoryDTO
                                {
                                    IdConsultories = Convert.ToInt32(reader["ID_CONSULTORIES"]),
                                    NumberConsultories = reader["NUMBER_CONSULTORIES"].ToString(),
                                    FloorNumber = Convert.ToInt32(reader["FLOOR_NUMBER"]),
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
                Console.WriteLine($"Error al obtener consultorio por ID: {ex.Message}");
            }
            return consultory;
        }

        public async Task<int> CreateConsultory(CreateConsultoryDTO consultory)
        {
            int idConsultory = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_CREATE_CONSULTORY", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NUMBER_CONSULTORIES", consultory.NumberConsultories);
                        cmd.Parameters.AddWithValue("@FLOOR_NUMBER", consultory.FloorNumber);
                        cmd.Parameters.AddWithValue("@ID_SPECIALTY", consultory.IdSpecialty);

                        var result = await cmd.ExecuteScalarAsync();
                        idConsultory = result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear consultorio: {ex.Message}");
            }
            return idConsultory;
        }

        public async Task<bool> UpdateConsultory(UpdateConsultoryDTO consultory)
        {
            bool success = false;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_UPDATE_CONSULTORY", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_CONSULTORIES", consultory.IdConsultories);
                        cmd.Parameters.AddWithValue("@NUMBER_CONSULTORIES", consultory.NumberConsultories);
                        cmd.Parameters.AddWithValue("@FLOOR_NUMBER", consultory.FloorNumber);
                        cmd.Parameters.AddWithValue("@ID_SPECIALTY", consultory.IdSpecialty);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar consultorio: {ex.Message}");
            }
            return success;
        }

        public async Task<bool> DeleteConsultory(int idConsultory)
        {
            bool success = false;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("SP_DELETE_CONSULTORY", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_CONSULTORIES", idConsultory);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar consultorio: {ex.Message}");
            }
            return success;
        }


    }
}
