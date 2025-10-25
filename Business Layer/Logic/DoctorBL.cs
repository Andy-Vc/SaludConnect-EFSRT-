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

        public Task<bool> AddDoctorSpecialty(DoctorSpecialtyDTO doctorSpecialty)
        {
            return service.AddDoctorSpecialty(doctorSpecialty);
        }

        public Task<int> CreateDoctor(CreateDoctorDTO doctor)
        {
            return service.CreateDoctor(doctor);
        }

        public Task<bool> DeleteDoctor(int idDoctor)
        {
            return service.DeleteDoctor(idDoctor);
        }

        public Task<DoctorDetailDTO> GetDoctorById(int idDoctor)
        {
            return service.GetDoctorById(idDoctor);
        }

        public async Task<DoctorCard> GetDoctorInfo(int idDoctor)
        {
            return await service.GetDoctorInfo(idDoctor);
        }

     
        public Task<List<DoctorFullDTO>> ListDoctors()
        {
            return service.ListDoctors();
        }

        public async Task<List<DoctorCard>> ListDoctorsWithExperience(int idSpeciality)
        {
            return await service.ListDoctorsWithExperience(idSpeciality);
        }

        public Task<bool> UpdateDoctor(UpdateDoctorDTO doctor)
        {
            return service.UpdateDoctor(doctor);
        }
    }
}
