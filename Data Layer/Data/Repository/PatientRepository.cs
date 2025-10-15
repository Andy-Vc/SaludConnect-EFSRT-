using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.DTO;

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
        public async Task<int> CountAppointments(int idPatient)
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
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return count;
        }
        public async Task<int> CountAppointmentsAssisted(int idPatient)
        {
            int count = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_total_citas_asistidas", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idPaciente", idPatient);
                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return count;
        }
        public async Task<int> CountAppointmentsCanceled(int idPatient)
        {
            int count = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_total_citas_canceladas", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idPaciente", idPatient);
                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return count;
        }
        public async Task<int> CountAppointmentsEarring(int idPatient)
        {
            int count = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_total_citas_pendientes", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idPaciente", idPatient);
                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return count;
        }
        public async Task<int> TotalDoctors()
        {
            int count = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(conexion))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_total_doctores", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        count = (int)await cmd.ExecuteScalarAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return count;
        }
        public async Task<List<UpcomingAppointments>> UpcomingAppointmentsPatient(int idPatient)
        {
            List<UpcomingAppointments> listUComAppoint = new List<UpcomingAppointments>();

            try
            {
                using (SqlConnection cn = new SqlConnection(conexion))
                {
                    await cn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_proximas_citas", cn))
                    {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idPaciente", idPatient);

                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            if (r.HasRows)
                            {
                                while (r.Read())
                                {
                                    listUComAppoint.Add(new UpcomingAppointments()
                                    {
                                        idAppointments = r.GetInt32(0),
                                        fullNames = r.GetString(1),
                                        specialty = r.GetString(2),
                                        hourAppointments = r.GetString(3),
                                        dateAppointments = r.GetString(4),
                                    });
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return listUComAppoint;
        }
        public async Task<List<PatientInformation>> PatientInformation(int idUser)
        {
            List<PatientInformation> listPatInfo = new List<PatientInformation>();
            try 
            {
                using (SqlConnection cnx = new SqlConnection(conexion))
                {
                    await cnx.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_patient_information", cnx))
                    {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idUser", idUser);
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.HasRows)
                            {
                                while (rd.Read())
                                {
                                    listPatInfo.Add(new PatientInformation()
                                    {
                                        idUser = rd.GetInt32(0),
                                        firstName = rd.GetString(1),
                                        lastNamePat = rd.GetString(2),
                                        lastNameMat = rd.GetString(3),
                                        document = rd.GetString(4),
                                        phone = rd.GetString(5),
                                        email = rd.GetString(6),
                                        namesContact = rd.IsDBNull(7) ? "" : rd.GetString(7),
                                        lastNamePatContact = rd.IsDBNull(8) ? "" : rd.GetString(8),
                                        lastNameMatContact = rd.IsDBNull(9) ? "" : rd.GetString(9),
                                        idRelationShip = rd.GetInt32(10),
                                        description_relationShip = rd.GetString(11),
                                    });
                                }
                            }
                        }
                    }
                }
            } 
            catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return listPatInfo;

        }
    }
}
