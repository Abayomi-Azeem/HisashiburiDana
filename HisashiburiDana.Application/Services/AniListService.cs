using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Abstractions.Infrastucture.ThirdPartyDependencies;
using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Services
{
    public class AniListService : IAniListService
    {
        private readonly IAnimeListManager _animelistManager;

        public AniListService(IAnimeListManager animelistManager)
        {
            _animelistManager = animelistManager;
        }


        public async Task<GeneralResponseWrapper<AnimeList>> GetAnimes(int pageNumber)
        {
            var response = new GeneralResponseWrapper<AnimeList>();

            var animes =  await _animelistManager.GetAnimes(pageNumber.ToString());

            if (response == null)
            {
                var errors = new List<string>()
                {
                    "An Error Occurred"
                };
                return response.BuildFailureResponse(errors);
            }
             return response.BuildSuccessResponse(animes);
        }


        public async Task<GeneralResponseWrapper<AllGenres>> GetAllGenres()
        {
            var response = new GeneralResponseWrapper<AllGenres>();

            var genres = await _animelistManager.GetAllGenres();

            if (response == null)
            {
                var errors = new List<string>()
                {
                    "An Error Occurred"
                };
                return response.BuildFailureResponse(errors);
            }
            return response.BuildSuccessResponse(genres);
        }
    }
}
