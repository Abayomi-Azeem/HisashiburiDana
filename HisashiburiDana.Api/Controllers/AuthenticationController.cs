using Amazon.DynamoDBv2.Model;
using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Contract.Authentication;
using HisashiburiDana.Contract.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HisashiburiDana.Api.Controllers
{
    /// <summary>
    /// Handles User Authentication Operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

      
        /// <summary>
        /// Register as a New User on the PlatForm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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



        /// <summary>
        /// Login on the Platform
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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



        /// <summary>
        /// Use the Refresh Token to Get a new Access Token
        /// </summary>
        /// <returns></returns>
        [Route("reissuetoken")]
        [HttpPost]
        public async Task<IActionResult> ReIssueToken(RefreshTokenRequest request)
        {
            var loginResponse = await _authService.ReIssueAccessToken(request);
            if (loginResponse.HasError)
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }


        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("forgotpassword")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email != userEmail.Value)
            {
                return BadRequest("email is not attached to this account.\nkindly input the email attached to this account");
            }
            var response = await _authService.SendCodeToEmail(email);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name=" request"></param>
        /// <returns></returns>
        [Route("resetpassword")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized("Unauthorized request, kindly log in");
            }
            var response = await _authService.ChangePassword(request,email.Value);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


        /// <summary>
        /// Verify code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("verifycode")]
        [HttpPost]
        [Produces(typeof(GeneralResponseWrapper<LoginResponse>))]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if(email == null)
            {
                return BadRequest("OOps something went wrong");
            }
            var response = await _authService.ValidateCode(code,email.Value);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
