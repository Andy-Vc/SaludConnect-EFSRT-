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
    public class SpecialtyRepository : ISpecialty
    {
        private readonly IConfiguration configuration;
        private readonly string stringConexion;

        public SpecialtyRepository(IConfiguration configuration)
        {

            this.configuration = configuration;
            stringConexion = configuration["ConnectionStrings:DB"];
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
    }
}
