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

namespace HisashiburiDana.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponseWrapper<bool?>> RegisterNewUser(RegisterRequest request)
        {
            GeneralResponseWrapper<bool?> response = new();
            
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
            var foundUser =  _unitOfWork.UserRepo.Get("Email", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, request.Email).Result;
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

        
    }
}
