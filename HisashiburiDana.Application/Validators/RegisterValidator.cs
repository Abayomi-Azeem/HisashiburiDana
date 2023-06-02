using FluentValidation;
using HisashiburiDana.Contract.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
