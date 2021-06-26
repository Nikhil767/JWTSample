using JWTSample.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.Interface
{
    public interface ILoginService
    {
        Task<LoginResponseDTO> UserLogin(LoginRequestDTO loginRequestDTO);
    }
}
