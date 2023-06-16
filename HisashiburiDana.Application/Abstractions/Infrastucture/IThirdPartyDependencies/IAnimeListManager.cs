using HisashiburiDana.Contract.AnimeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.ThirdPartyDependencies
{
    public interface IAnimeListManager
    {
        Task<AnimeList> GetAnimes(string pageNumber);

        Task<AllGenres?> GetAllGenres();
        Task<AnimeList> GetTrendingAnimes();
        Task<AnimeList> SearchAnimes(string animeName);
    }
}
