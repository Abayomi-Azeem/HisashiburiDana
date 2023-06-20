using HisashiburiDana.Contract.Authentication;
using HisashiburiDana.Contract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Application
{
    public interface IAuthenticationService
    {
        Task<GeneralResponseWrapper<LoginResponse>> LoginUser(LoginRequest request);
        Task<GeneralResponseWrapper<bool?>> RegisterNewUser(RegisterRequest request);
        Task<GeneralResponseWrapper<string>> ReIssueAccessToken(RefreshTokenRequest request);
        Task<GeneralResponseWrapper<bool?>> SendCodeToEmail(string email);
        Task<GeneralResponseWrapper<bool?>> ChangePassword(ResetPasswordRequest request, string email);
        Task<GeneralResponseWrapper<bool?>> ValidateCode(string code, string email);

    }
}
