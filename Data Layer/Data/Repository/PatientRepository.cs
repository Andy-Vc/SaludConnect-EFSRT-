using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
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
        public async Task<PatientInformation> PatientInformation(int idUser)
        {
            var patientInformation = new PatientInformation();
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
                                    patientInformation = new PatientInformation
                                    {
                                        idUser = rd.GetInt32(0),
                                        firstName = rd.GetString(1),
                                        lastNamePat = rd.GetString(2),
                                        lastNameMat = rd.GetString(3),
                                        document = rd.GetString(4),
                                        phone = rd.GetString(5),
                                        email = rd.GetString(6),
                                        dateRegister = rd.GetDateTime(7),
                                        imageProfile = rd.IsDBNull(8) ? "" : rd.GetString(8),
                                        namesContact = rd.IsDBNull(9) ? "" : rd.GetString(9),
                                        lastNamePatContact = rd.IsDBNull(10) ? "" : rd.GetString(10),
                                        lastNameMatContact = rd.IsDBNull(11) ? "" : rd.GetString(11),
                                        idRelationShip = rd.GetInt32(12),
                                        phoneContactEmergency = rd.GetString(13),
                                        description_relationShip = rd.GetString(14),
                                    };
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
            return patientInformation;

        }
        public async Task<List<RelationShip>> CompletListOfRelationShips()
        {
            List<RelationShip> listRelation = new List<RelationShip>();

            using (SqlConnection cn = new SqlConnection(conexion))
            { 
                await cn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TB_RELATIONSHIP", cn)) {
                    using (SqlDataReader rd = cmd.ExecuteReader()) {
                        if (rd.HasRows) {
                            while (rd.Read()) {
                                listRelation.Add(new RelationShip() 
                                {
                                    idRelationShip = rd.GetInt32(0),
                                    descriptionRelationShip = rd.GetString(1)
                                }); 
                            }
                        }
                    }
                }
            }

            return listRelation;
        }
        public async Task<PatientUpdate> UpdateInformationPatient(PatientUpdate patient, int idUser)
        {
            PatientUpdate updatePatient = new PatientUpdate();

            using (SqlConnection cn = new SqlConnection(conexion)) 
            { 
                await cn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_update_information_patient", cn)) { 
                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idUser",   idUser);
                    cmd.Parameters.AddWithValue("@firstname", patient.firstName);
                    cmd.Parameters.AddWithValue("@lastNamePat", patient.lastNamePat);
                    cmd.Parameters.AddWithValue("@lastNameMat", patient.lastNameMat);
                    cmd.Parameters.AddWithValue("@document", patient.document);
                    cmd.Parameters.AddWithValue("@phone", patient.phone);
                    cmd.Parameters.AddWithValue("@email", patient.email);
                    cmd.Parameters.AddWithValue("@profilePicture", patient.imageProfile );
                    //parameters contact
                    cmd.Parameters.AddWithValue("@idContact", patient.Emergency.idEContact);
                    cmd.Parameters.AddWithValue("@namesContact", patient.Emergency.namesContact);
                    cmd.Parameters.AddWithValue("@contacNamePat", patient.Emergency.lastNamePat);
                    cmd.Parameters.AddWithValue("@contacNameMat", patient.Emergency.lastNameMat);
                    cmd.Parameters.AddWithValue("@idRelation", patient.Emergency.relationShip.idRelationShip);
                    cmd.Parameters.AddWithValue("@phoneEmergency", patient.Emergency.phoneEmergency);

                    await cmd.ExecuteNonQueryAsync();

                    updatePatient = patient;
                }
            }
            return updatePatient;
        }
        public async Task<List<PatientNextAppointements>> PatientNextAppointement(int idPatient) {

            var listNextAppointment = new List<PatientNextAppointements>();
            
            try {
                using (SqlConnection c = new SqlConnection(conexion))
                {
                    await c.OpenAsync();
                    using (SqlCommand cd = new SqlCommand("sp_proximas_citas", c))
                    {

                        cd.CommandType = System.Data.CommandType.StoredProcedure;

                        cd.Parameters.AddWithValue("@idPaciente", idPatient);

                        using (SqlDataReader rd = await cd.ExecuteReaderAsync())
                        {
                            if (rd.HasRows)
                            {
                                while (await rd.ReadAsync())
                                {
                                    listNextAppointment.Add(new PatientNextAppointements
                                    {
                                        idAppointment = rd.GetInt32(0),
                                        nombresCompletos = rd.GetString(1),
                                        nameSpeciality = rd.GetString(2),
                                        horaCita = rd.GetString(3),
                                        fechaCita = rd.GetString(4),
                                        state = rd.GetString(5),
                                        appointmentPrice = rd.GetDecimal(6),
                                        numberConsultories = rd.GetString(7),
                                        floor_number = rd.GetInt32(8)
                                    });
                                }
                            }
                        }
                    }
               
                }

            } catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return listNextAppointment;
        }
    }
}
