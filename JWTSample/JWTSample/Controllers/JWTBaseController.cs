using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTBaseController : ControllerBase
    {

        protected IActionResult HandleUserException(Exception ex)
        {
            return BadRequest(ex.Message);
        }

        protected IActionResult HandleOtherException(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }
}