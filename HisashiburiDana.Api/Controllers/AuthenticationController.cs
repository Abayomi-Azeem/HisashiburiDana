using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Contract.Authentication;
using HisashiburiDana.Contract.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HisashiburiDana.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        //register
        [Route("register")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            var registerResponse = await _authService.RegisterNewUser(request);
            if (registerResponse.HasError)
            {
                return BadRequest(registerResponse);
            }
            return Ok(registerResponse);
            
        }


        [Route("login")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<LoginResponse>))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginResponse = await _authService.LoginUser(request);
            if (loginResponse.HasError)
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }
        //login

        
    }
}
