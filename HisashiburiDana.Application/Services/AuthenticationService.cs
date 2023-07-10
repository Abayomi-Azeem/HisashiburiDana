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
using Amazon.Runtime.Internal;
using HisashiburiDana.Application.Abstractions.Infrastucture.IEmailService;
using HisashiburiDana.Contract.EmailManager;

namespace HisashiburiDana.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGeneratior;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IEmailSender _emailService;


        public AuthenticationService(IUnitOfWork unitOfWork, ITokenGenerator tokenGeneratior, ILogger<AuthenticationService> logger, IEmailSender emailService)
        {
            _unitOfWork = unitOfWork;
            _tokenGeneratior = tokenGeneratior;
            _logger = logger;
            _emailService = emailService;
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

        public async Task<GeneralResponseWrapper<bool?>> SendCodeToEmail(string email)
        {
            _logger.LogInformation($"SendCodeToEmail Request Arrived ---{email}");
            GeneralResponseWrapper<bool?> response = new(_logger);

            try
            {
                //get user from data
                var user = await _unitOfWork.UserRepo.Get("Email", ScanOperator.Equal, email);
                if (user == null)
                {
                    List<string> errors = new()
                    {
                      "An Otp has been sent to your email address, if the email address provided is correct."
                    };
                    return response.BuildFailureResponse(errors);
                }

                //* Generate random code and send to email address
                var code = GenerateRandomCode();

                //* Send code to email
                var status = _emailService.SendPasswordResetEmail(email, code);
                if (!status)
                {
                    List<string> errors = new()
                    {
                     "an error occurred while sending passcode to email"
                    };
                    return response.BuildFailureResponse(errors);
                }

                //* Saving the code in the user's record in the database
                //
                var codeExpiration = DateTime.Now.AddMinutes(5);
               // var updateCode = User.UpdateCode(code, codeExpiration);
                var updateCode = User.UpdateCode(user,code ,codeExpiration);
                await _unitOfWork.UserRepo.Update(updateCode);
                _logger.LogInformation($"Updated Pass Code Successfully for User ---{email}");

                //* deeplink that takes you back to page to verify code sent is code inputted

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception occurred in Updating PassCode and CodeExpiration -- {ex.Message}--\n {ex.StackTrace}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Updating PassCode and CodeExpiration" });
            }        
         }

        public async Task<GeneralResponseWrapper<bool?>> ChangePassword(ResetPasswordRequest request, string email)
        {

            _logger.LogInformation($"ChangePassword Request Arrived ---{email}");
            GeneralResponseWrapper<bool?> response = new(_logger);

            //validating request
            var validator = new ResetPasswordRequestValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validator.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return response.BuildFailureResponse(errors);
            }

            try
            {

                var user = _unitOfWork.UserRepo.Get("Email", ScanOperator.Equal, email).Result; 
                if(user == null)
                {
                    List<string> errors = new()
                    {
                        "user doesn't exist"
                    };
                    return response.BuildFailureResponse(errors);
                }

                //salt and hash password
                var passwordSalt = PasswordHasher.GeneratePasswordSalt(user.FirstName, user.LastName);
                var passwordHash = PasswordHasher.HashPassword(request.NewPassword, passwordSalt);

                //* updating password column in user table
                var updatePassword = User.UpdatePassword(user, passwordHash, passwordSalt);
                await _unitOfWork.UserRepo.Update(updatePassword);
                _logger.LogInformation($"Updated Password Successfully for User ---{email}");

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in Updating Password -- {ex.Message}--\n {ex.StackTrace}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Updating Password" });
            }

        }

        public async Task<GeneralResponseWrapper<bool?>> ValidateCode(string code)
        {
            _logger.LogInformation($"ValidateCode Request Arrived --- | DATE: {DateTime.Now:dd MMM yyyy : HH-mm}");
            GeneralResponseWrapper<bool?> response = new(_logger);
            try
            {
                //* check that code is the same as the one sent to the user's email
                var user = _unitOfWork.UserRepo.Get("PassCode", ScanOperator.Equal, code).Result;
                if(user == null)
                {
                    List<string> errors = new()
                    {
                        "invalid user"
                    };
                    return response.BuildFailureResponse(errors);
                }
                if (user.PassCode != code || DateTime.Now > user.CodeExpiration)
                {
                    List<string> errors = new()
                    {
                        "The code you provided is invalid"
                    };
                    return response.BuildFailureResponse(errors);
                }

                //reset Passcode and CodeExpiration to default
                var updateCode = User.UpdateCode(user,null,DateTime.MinValue);
                await _unitOfWork.UserRepo.Update(updateCode);
                _logger.LogInformation($"Updated PassCode and CodeExpiration Successfully | DATE: {DateTime.Now:dd MMM yyyy : HH-mm}");
                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception occurred in Updating PassCode and CodeExpiration -- {ex.Message}--\n {ex.StackTrace}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Updating PassCode and CodeExpiration" });
            }
 

        }

        private string GenerateRandomCode()
        {
            const string chars = "0123456789";
            var random = new Random();
            var code = new char[6];

            for (var i = 0; i < code.Length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }

            return new string(code);
        }
    }


}
