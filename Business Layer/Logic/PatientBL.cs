using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;

namespace Logic
{
    public class PatientBL : IPatient
    {
        private readonly IPatient _patient;

        public PatientBL(IPatient service)
        {
            this._patient = service;
        }
        public async Task<int> CountAppoitments(int idPatient)
        {
            return await _patient.CountAppoitments(idPatient);
        }
    }
}
