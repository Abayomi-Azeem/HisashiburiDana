using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence;
using HisashiburiDana.Application.Utils;
using HisashiburiDana.Application.Validators;
using HisashiburiDana.Contract.Authentication;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using HisashiburiDana.Application.Abstractions.Infrastucture.Authentication;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace HisashiburiDana.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGeneratior;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IUnitOfWork unitOfWork, ITokenGenerator tokenGeneratior, ILogger<AuthenticationService> logger)
        {
            _unitOfWork = unitOfWork;
            _tokenGeneratior = tokenGeneratior;
            _logger = logger;
        }

        public async Task<GeneralResponseWrapper<LoginResponse>> LoginUser(LoginRequest request)
        {
            _logger.LogInformation($"Login Request Arrived ---{request.Email}");
            GeneralResponseWrapper<LoginResponse> response = new(_logger);
            //validate request
            var validator = new LoginRequestValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validator.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return response.BuildFailureResponse(errors);
            }


            //get user from data
            var user = await _unitOfWork.UserRepo.Get("Email", ScanOperator.Equal, request.Email);
            if (user == null)
            {
                List<string> errors = new()
                {
                    "Password or UserName Incorrect"
                };
                return response.BuildFailureResponse(errors);
            };


            //hash and salt password
            var passwordHash = PasswordHasher.HashPassword(request.Password, user.PasswordSalt);


            //check if password is the same
            if (passwordHash != user.Password)
            {
                List<string> errors = new()
                {
                    "Password or UserName Incorrect"
                };
                return response.BuildFailureResponse(errors);
            }

            var accessToken = _tokenGeneratior.GenerateAccessToken(user);
            var refreshToken = _tokenGeneratior.GenerateRefreshToken();

           
            var loginResponse = new LoginResponse()
            {
                Email = user.Email,
                Id  = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return response.BuildSuccessResponse(loginResponse); 
            

        }

        public async Task<GeneralResponseWrapper<bool?>> RegisterNewUser(RegisterRequest request)
        {
            _logger.LogInformation($"Registration Request Arrived ---{request.Email}");
            GeneralResponseWrapper<bool?> response = new(_logger);
            
            //validate request
            var validator = new RegisterValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validator.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                
                return response.BuildFailureResponse(errors); 
            }

            //check if user already exists
            var foundUser =  _unitOfWork.UserRepo.Get("Email", ScanOperator.Equal, request.Email).Result;
            if (foundUser != null)
            {
                List<string> errors  = new()
                {
                    "User already Exists"
                };                
                return response.BuildFailureResponse(errors);
            }

            //salt and hash password
            var passwordSalt = PasswordHasher.GeneratePasswordSalt(request.FirstName, request.LastName);
            var passwordHash = PasswordHasher.HashPassword(request.Password, passwordSalt);


            //create user
            User createdUser = User.Create(request.FirstName, request.LastName, request.Email, passwordHash, passwordSalt);
            await _unitOfWork.UserRepo.InsertAsync(createdUser);
            
            return response.BuildSuccessResponse(true);
        }

        public async Task<GeneralResponseWrapper<string>> ReIssueAccessToken(RefreshTokenRequest request)
        {
            _logger.LogInformation($"ReIssue Token Request Arrived ---{request.RefreshToken}");
            GeneralResponseWrapper<string> response = new(_logger);

            bool isRefreshTokenValid = _tokenGeneratior.ValidateRefreshoken(request.RefreshToken);

            if (!isRefreshTokenValid)
            {
                List<string> errors = new()
                {
                    "Invalid Refresh Token"
                };
                return response.BuildFailureResponse(errors);
            }

            var accessTokenClaimsPrincipal = _tokenGeneratior.ValidateAccessTokenWithoutLifetime(request.AccessToken);

            if (!accessTokenClaimsPrincipal.Identity.IsAuthenticated)
            {
                List<string> errors = new()
                {
                    "Invalid Access Token"
                };
                return response.BuildFailureResponse(errors);
            }

            string userId = accessTokenClaimsPrincipal.Claims.FirstOrDefault(x => x.Type.ToLower().Contains("nameid")).Value;

            User? user = await _unitOfWork.UserRepo.Get("Id", ScanOperator.Equal, userId);

            if (user == null)
            {
                List<string> errors = new()
                {
                    "User does not exist"
                };
                return response.BuildFailureResponse(errors);
            }

            var newAccessToken = _tokenGeneratior.GenerateAccessToken(user);

            return response.BuildSuccessResponse(newAccessToken);

        }
    }
}
