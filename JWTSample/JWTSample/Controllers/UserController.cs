using System;
using System.Threading.Tasks;
using JWTSample.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTSample.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : JWTBaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> UserData([FromRoute] string userName)
        {
            try
            {
                var response = await _userService.GetUserDetails(userName);
                return Ok(response);
            }
            catch (ArgumentException ex)
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