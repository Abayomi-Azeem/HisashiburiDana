using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence;
using HisashiburiDana.Application.Validators;
using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Contract.Users;
using HisashiburiDana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Services
{
    public class UserAnimeService : IUserAnimeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAnimeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponseWrapper<bool>> AddNewAnimeToWatchList(AddAnimeFromAniList request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new AddAnimeFromAnilistValidator().Validate(request);

            if (!validator.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validator.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return response.BuildFailureResponse(errors);
            }

            try
            {
                var addtoWatchlistentity = ToWatchAnimes.Create(request);
                List<string> rankiIds = new();
                foreach (var ranking in request.Media.Rankings)
                {
                    var rank = Rankings.Create(ranking, request.UserId);
                    await _unitOfWork.AnimeRankingsRepo.InsertAsync(rank);
                    rankiIds.Add(rank.Id);
                }
                var addtoWatchlistentitywithRanks = addtoWatchlistentity.AddRankingId(addtoWatchlistentity, rankiIds);
                await _unitOfWork.ToWatchAnimeRepo.InsertAsync(addtoWatchlistentitywithRanks);
                return response.BuildSuccessResponse(true);
                 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in AddNewAnimeWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
                 
            }
        }
    
        public async Task<GeneralResponseWrapper<bool>> AddNewAnimeToAlreadyWatched(AddAnimeFromAniList request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new AddAnimeFromAnilistValidator().Validate(request);

            if (!validator.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validator.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return response.BuildFailureResponse(errors);
            }

            try
            {
                var addToAlreadyWatched = WatchedAnimes.Create(request);
                List<string> rankiIds = new();
                foreach (var ranking in request.Media.Rankings)
                {
                    var rank = Rankings.Create(ranking, request.UserId);
                    await _unitOfWork.AnimeRankingsRepo.InsertAsync(rank);
                    rankiIds.Add(rank.Id);
                }
                var addtowatchedwithRanks = addToAlreadyWatched.AddRankingId(addToAlreadyWatched, rankiIds);
                await _unitOfWork.WatchedAnimeRepo.InsertAsync(addtowatchedwithRanks);
                return response.BuildSuccessResponse(true);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in AddNewAnimeWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });

            }



        }
        
        public async Task<GeneralResponseWrapper<bool>> AddNewAnimeToCurrentlyWatching(AddAnimeFromAniList request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new AddAnimeFromAnilistValidator().Validate(request);

            if (!validator.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validator.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return response.BuildFailureResponse(errors);
            }

            try
            {
                var addToCurrentlyWatching = WatchingAnimes.Create(request);
                List<string> rankiIds = new();
                foreach (var ranking in request.Media.Rankings)
                {
                    var rank = Rankings.Create(ranking, request.UserId);
                    await _unitOfWork.AnimeRankingsRepo.InsertAsync(rank);
                    rankiIds.Add(rank.Id);
                }
                var addtowatchingwithRanks = addToCurrentlyWatching.AddRankingId(addToCurrentlyWatching, rankiIds);
                await _unitOfWork.WatchingAnimeRepo.InsertAsync(addtowatchingwithRanks);
                return response.BuildSuccessResponse(true);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in AddNewAnimeWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });

            }
        }
    }
}

