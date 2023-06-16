﻿using HisashiburiDana.Application.Abstractions.Application;
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
    
        
    
    }
}
