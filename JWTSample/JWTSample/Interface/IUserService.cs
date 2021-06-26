using JWTSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.Interface
{
    public interface IUserService
    {
        Task<User> GetUserDetails(string userName);
    }
}
