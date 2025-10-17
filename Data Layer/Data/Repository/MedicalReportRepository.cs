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
    public class MedicalReportRepository : IMedicalRecord
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public MedicalReportRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }
        public async Task<ResultResponse<int>> RegisterRecordWithServicesAsync(MedicalRecord record)
        {
            var result = new ResultResponse<int>();

            try
            {
                using var connection = new SqlConnection(stringConexion);
                await connection.OpenAsync();

                using var command = new SqlCommand("SP_INSERT_RECORD_WITH_SERVICES", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdAppointment", record.IdAppointment);
                command.Parameters.AddWithValue("@Observations", record.Observations ?? string.Empty);
                command.Parameters.AddWithValue("@Diagnosis", (object)record.Diagnosis ?? DBNull.Value);
                command.Parameters.AddWithValue("@Treatment", (object)record.Treatment ?? DBNull.Value);

                var serviceIds = record.AdditionalServices?.Select(s => s.Service.IdService).ToList() ?? new List<int>();
                string serviceIdsCsv = string.Join(",", serviceIds);
                command.Parameters.AddWithValue("@ServiceIds", serviceIdsCsv);

                var outputParam = new SqlParameter("@NewRecordID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                await command.ExecuteNonQueryAsync();

                var newId = outputParam.Value;
                if (newId != null && int.TryParse(newId.ToString(), out int idRecord))
                {
                    result.Value = true;
                    result.Data = idRecord;
                }
                else
                {
                    result.Value = false;
                    result.Message = "No se pudo obtener el ID del registro creado.";
                }
            }
            catch (Exception ex)
            {
                result.Value = false;
                result.Message = "Error: " + ex.Message;
            }


            return result;
        }


    }
}
