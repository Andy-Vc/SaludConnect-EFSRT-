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
    public class AuthorizationBL : IAuthorization
    {
        private readonly IAuthorization service;

        public AuthorizationBL(IAuthorization service)
        {
            this.service = service;
        }

        public async Task<ResultResponse<User>> Login(string email, string password)
        {
            return await service.Login(email, password);
        }
    }
}
