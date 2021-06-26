using System;
using System.Threading.Tasks;
using JWTSample.DTO;
using JWTSample.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTSample.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : JWTBaseController
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var response = await _loginService.UserLogin(loginRequestDTO);
                return Ok(response);
            }
            catch(ArgumentException ex)
            {
                return HandleUserException(ex);
            }
            catch (Exception ex)
            {
                return HandleOtherException(ex);
            }
        }
    }
}