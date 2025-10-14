using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Repository
{
    public class PatientRepository : IPatient
    {
        private readonly IConfiguration configuration;
        private readonly string conexion;

        public PatientRepository(IConfiguration config)
        {
            this.configuration = config;
            conexion = configuration["ConnectionStrings:DB"];
        }
        public async Task<int> CountAppoitments(int idPatient)
        {
            int count = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_total_citas", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idPaciente", idPatient);
                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine("Error: "+ ex.Message);
            }
                return count;
        }
    }
}
