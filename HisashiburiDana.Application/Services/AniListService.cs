using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Abstractions.Infrastucture.ThirdPartyDependencies;
using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Common;
using Microsoft.Extensions.Logging;
using Serilog;
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
        private readonly ILogger<AniListService> _logger;

        public AniListService(IAnimeListManager animelistManager, ILogger<AniListService> logger)
        {
            _animelistManager = animelistManager;
            _logger = logger;
        }


        public async Task<GeneralResponseWrapper<AnimeList>> GetAnimes(int pageNumber)
        {
            _logger.LogInformation($"Get Animes Request arrived -----Page Number: {pageNumber}");
            var response = new GeneralResponseWrapper<AnimeList>(_logger);

            var animes =  await _animelistManager.GetAnimes(pageNumber.ToString());

            if (animes == null)
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
            _logger.LogInformation($"GetAllGenres Request arrived -----");
            var response = new GeneralResponseWrapper<AllGenres>(_logger);

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

        public async Task<GeneralResponseWrapper<AnimeList>> GetTrendingAnime()
        {
            _logger.LogInformation($"Get Trending Animes Request arrived -----");
            var response = new GeneralResponseWrapper<AnimeList>(_logger);

            var animes = await _animelistManager.GetTrendingAnimes();

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

        public async Task<GeneralResponseWrapper<AnimeList>> SearchInAnimeList(string animeName, int pageNumber)
        {
            var response = new GeneralResponseWrapper<AnimeList>();
            var mediumList = new List<Medium>();
            var animes = await _animelistManager.GetAnimes(pageNumber.ToString());
            var pageinfo = animes.Page.PageInfo;
            var lastPage = pageinfo.LastPage;
            var currentPage = pageinfo.CurrentPage;
            var searchResults = animes.Page.Media;
            var hasnextpage = pageinfo.HasNextPage;

            foreach (var anime in searchResults)
            {

                if (anime.Title.English != null && anime.Title.Romaji != null)
                {
                    if (anime.Title.English.Contains(animeName) || anime.Title.Romaji.Contains(animeName))
                    {
                        mediumList.Add(anime);
                    }
                }

            }

            while (hasnextpage  == true && currentPage <= lastPage)
            {
                pageNumber++; // Move to the next page
                animes = await _animelistManager.GetAnimes(pageNumber.ToString());
                searchResults = animes.Page.Media;
                hasnextpage = animes.Page.PageInfo.HasNextPage;
                foreach (var anime in searchResults)
                {
                    if (anime.Title.English != null && anime.Title.Romaji != null)
                    {
                        if (anime.Title.English.Contains(animeName) || anime.Title.Romaji.Contains(animeName))
                        {
                            mediumList.Add(anime);
                        }
                    }
                }
            }
            var animelist = new AnimeList
            {
                Page = new Page
                {
                    Media = mediumList,
                    PageInfo = pageinfo
                }
            };

            if (response == null)
            {
                var errors = new List<string>()
                {
                    "An Error Occurred"
                };
                return response.BuildFailureResponse(errors);
            }
            return response.BuildSuccessResponse(animelist);

        }
    }
}
