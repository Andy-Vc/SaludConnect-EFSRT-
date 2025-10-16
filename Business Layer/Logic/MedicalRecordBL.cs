using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Models;
using Models.DTO;

namespace Logic
{
    public class MedicalRecordBL : IMedicalRecord
    {
        private readonly IMedicalRecord service;

        public MedicalRecordBL(IMedicalRecord service)
        {
            this.service = service;
        }
        public async Task<ResultResponse<int>> RegisterRecordWithServicesAsync(MedicalRecord record)
        {
            return await service.RegisterRecordWithServicesAsync(record);
        }
    }
}
