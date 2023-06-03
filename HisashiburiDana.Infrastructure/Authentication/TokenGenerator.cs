using HisashiburiDana.Application.Abstractions.Infrastucture.Authentication;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings _settings;

        public TokenGenerator(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateAccessToken(User user)
        {
            var signInCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
            };

            var securityToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1).AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: signInCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public string GenerateRefreshToken()
        {
            var signInCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            List<Claim> claims = new();
            

            var securityToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1).AddHours(6),
            signingCredentials: signInCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public bool ValidateRefreshoken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)),
                ValidateIssuer = true, 
                ValidateAudience = true, 
                ValidateLifetime = true,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
               
            };

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
            return claimsPrincipal.Identity.IsAuthenticated;
        }


    }
}
