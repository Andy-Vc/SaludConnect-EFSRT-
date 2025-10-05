using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;

namespace Logic
{
    public class UserBL : IUser
    {
        private readonly IUser service;

        public UserBL(IUser service)
        {
            this.service = service;
        }

        public async Task<int> totalDoctors()
        {
            return await service.totalDoctors();
        }
    }
}
