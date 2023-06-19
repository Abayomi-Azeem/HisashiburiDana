using FluentValidation;
using HisashiburiDana.Contract.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Validators
{
    public class DeleteAnimeValidator : AbstractValidator<DeleteAnimeRequest>
    {
        public DeleteAnimeValidator()
        {
            RuleFor(x => x.AnimeId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
        }
    }
}
