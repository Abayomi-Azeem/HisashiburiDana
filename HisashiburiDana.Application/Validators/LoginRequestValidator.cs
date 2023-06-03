using FluentValidation;
using HisashiburiDana.Contract.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
