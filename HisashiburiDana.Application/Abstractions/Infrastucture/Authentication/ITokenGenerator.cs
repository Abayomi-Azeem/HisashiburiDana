using HisashiburiDana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.Authentication
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        bool ValidateRefreshoken(string refreshToken);
    }
}
