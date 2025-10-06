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
    public class UserBL : IUser
    {
        private readonly IUser service;

        public UserBL(IUser service)
        {
            this.service = service;
        }

        public async Task<ResultResponse<User>> GetProfile(int idUser)
        {
            return await service.GetProfile(idUser);
        }

        public async Task<int> totalDoctors()
        {
            return await service.totalDoctors();
        }
    }
}
