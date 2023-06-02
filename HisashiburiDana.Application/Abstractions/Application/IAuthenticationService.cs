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
        Task<GeneralResponseWrapper<bool?>> RegisterNewUser(RegisterRequest request);


    }
}
