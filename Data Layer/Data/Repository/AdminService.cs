using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Repository
{
    public class AdminService : IAdmin
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public AdminService(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
        }

        public async Task<int> GetTotalAppointments()
        {
            int total = 0;
            try
            {
                using (var conexion = new SqlConnection(stringConexion))
                {
                    await conexion.OpenAsync();

                    using (var cmd = new SqlCommand("SP_Total_Appointments", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        total = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el total de citas: {ex.Message}");
                total = 0;
            }

            return total;
        }

    }
}
