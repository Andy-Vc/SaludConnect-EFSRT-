using Data.Interface;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class DoctorBL : IDoctor
    {
        private readonly IDoctor service;

        public DoctorBL(IDoctor service)
        {
            this.service = service;
        }

        public async Task<DoctorCard> GetDoctorInfo(int idDoctor)
        {
            return await service.GetDoctorInfo(idDoctor);
        }

        public async Task<List<DoctorCard>> ListDoctorsWithExperience(int idSpeciality)
        {
            return await service.ListDoctorsWithExperience(idSpeciality);
        }
    }
}
