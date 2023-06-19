using HisashiburiDana.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.Authentication
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        bool ValidateRefreshoken(string refreshToken);

        ClaimsPrincipal ValidateAccessTokenWithoutLifetime(string accessToken);
    }
}
