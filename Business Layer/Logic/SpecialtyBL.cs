using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;

namespace Logic
{
    public class SpecialtyBL : ISpecialty
    {
        private readonly ISpecialty service;

        public SpecialtyBL(ISpecialty service)
        {
            this.service = service;
        }

        public async Task<int> totalSpecialties()
        {
            return await service.totalSpecialties();
        }
    }
}
