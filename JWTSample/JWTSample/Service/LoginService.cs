using JWTSample.DataContext;
using JWTSample.DTO;
using JWTSample.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTSample.Service
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly SampleDbContext _sampleDbContext;
        public LoginService(SampleDbContext sampleDbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _sampleDbContext = sampleDbContext;
        }

        public async Task<LoginResponseDTO> UserLogin(LoginRequestDTO loginRequestDTO)
        {            
            await ValidateLoginRequestDTO(loginRequestDTO);

            var currentUser = _sampleDbContext.Users.Local.FirstOrDefault(x => x.UserName.ToUpper() == loginRequestDTO.UserName.ToUpper() &&
                                                  x.Password == loginRequestDTO.Password);

            if(currentUser == null)
                throw new ArgumentException($"User with UserName : '{loginRequestDTO.UserName}' not found");

            LoginResponseDTO loginResponseDTO = await GenerateJWTToken(loginRequestDTO.UserName, currentUser.Role);
            return await Task.FromResult(loginResponseDTO);
        }

        private async Task<LoginResponseDTO> GenerateJWTToken(string userName, string role)
        {            
            var jwtTokenConfig = _configuration.GetSection("JWTTokenConfig");
            var secretKey = Encoding.ASCII.GetBytes(jwtTokenConfig.GetSection("Secret").Value);
            var audience = jwtTokenConfig.GetSection("Aud").Value;
            var issuer = jwtTokenConfig.GetSection("Iss").Value;

            int expiry = 0;
            if (!string.IsNullOrWhiteSpace(role) && role.ToUpper() == "ADMIN")
                expiry = jwtTokenConfig.GetValue<int>("AdminExpiry");
            else
                expiry = jwtTokenConfig.GetValue<int>("UserExpiry");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,                
                Subject = new ClaimsIdentity(new[] { new Claim("userName", userName) }),
                Expires = DateTime.UtcNow.AddMinutes(expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var originalToken = tokenHandler.WriteToken(securityToken);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO
            {
                UserName = userName,
                Token = originalToken
            };
            return await Task.FromResult(loginResponseDTO);
        }

        private async Task ValidateLoginRequestDTO(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
                throw new ArgumentException("loginRequestDTO is null");

            if (string.IsNullOrWhiteSpace(loginRequestDTO.UserName))
                throw new ArgumentException("username is null");

            if (string.IsNullOrWhiteSpace(loginRequestDTO.Password))
                throw new ArgumentException("password is null");
        }
    }
}
