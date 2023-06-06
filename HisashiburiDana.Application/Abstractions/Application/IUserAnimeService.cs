using HisashiburiDana.Contract.Common;
using HisashiburiDana.Contract.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Application
{
    public interface IUserAnimeService
    {
        Task<GeneralResponseWrapper<bool>> AddNewAnimeToWatchList(AddAnimeToWatchListRequest request);
    }
}
