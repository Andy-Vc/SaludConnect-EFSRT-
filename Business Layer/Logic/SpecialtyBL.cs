using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Models;

namespace Logic
{
    public class SpecialtyBL : ISpecialty
    {
        private readonly ISpecialty service;

        public SpecialtyBL(ISpecialty service)
        {
            this.service = service;
        }

        public async Task<List<Specialty>> ListSpecialitiesWithDescription()
        {
            return await service.ListSpecialitiesWithDescription();
        }

        public async Task<List<Specialty>> ListSpecialties()
        {
            return await service.ListSpecialties();
        }

        public async Task<int> totalSpecialties()
        {
            return await service.totalSpecialties();
        }

    }
}
