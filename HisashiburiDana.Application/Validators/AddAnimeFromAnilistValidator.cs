using FluentValidation;
using HisashiburiDana.Contract.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Validators
{
    public class AddAnimeFromAnilistValidator: AbstractValidator<AddAnimeFromAniList>
    {
        public AddAnimeFromAnilistValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Media).NotNull();
        }
    }
}
