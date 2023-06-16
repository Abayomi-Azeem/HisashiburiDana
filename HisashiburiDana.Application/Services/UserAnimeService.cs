using FluentValidation;
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
                var addtoWatchlistentitywithRanks = ToWatchAnimes.AddRankingId(addtoWatchlistentity, rankiIds);
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
                var addtowatchedwithRanks = WatchedAnimes.AddRankingId(addToAlreadyWatched, rankiIds);
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
                var addtowatchingwithRanks = WatchingAnimes.AddRankingId(addToCurrentlyWatching, rankiIds);
                await _unitOfWork.WatchingAnimeRepo.InsertAsync(addtowatchingwithRanks);
                return response.BuildSuccessResponse(true);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in AddNewAnimeWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });

            }
        }
    
        public async Task<GeneralResponseWrapper<bool>> MoveToCurrentlyWatchingFromWatchList(MoveAnimeWithinUserLists request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new MoveAnimeWithinUserListsValidator().Validate(request);

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
                
                ToWatchAnimes? anime = await _unitOfWork.ToWatchAnimeRepo.GetById(request.AnimeId);
                
                if (anime == null)
                {
                    List<string> errors = new()
                    {
                        "Anime Cannot Be Found"
                    };
                    return response.BuildFailureResponse(errors);
                }

                if (anime.UserId != request.UserId)
                {
                    List<string> errors = new()
                    {
                        "Anime cannot be found in User List"
                    };
                    return response.BuildFailureResponse(errors);
                }

                WatchingAnimes watchingAnime = WatchingAnimes.Create(anime);

                await _unitOfWork.WatchingAnimeRepo.InsertAsync(watchingAnime);

                await _unitOfWork.ToWatchAnimeRepo.Delete(anime);

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in MoveToCurrentlyWatchingFromWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToCurrentlyWatchingFromAlreadyWatched(MoveAnimeWithinUserLists request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new MoveAnimeWithinUserListsValidator().Validate(request);

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

                WatchedAnimes? anime = await _unitOfWork.WatchedAnimeRepo.GetById(request.AnimeId);

                if (anime == null)
                {
                    List<string> errors = new()
                    {
                        "Anime Cannot Be Found"
                    };
                    return response.BuildFailureResponse(errors);
                }

                if (anime.UserId != request.UserId)
                {
                    List<string> errors = new()
                    {
                        "Anime cannot be found in User List"
                    };
                    return response.BuildFailureResponse(errors);
                }

                WatchingAnimes watchedAnime = WatchingAnimes.Create(anime);

                await _unitOfWork.WatchingAnimeRepo.InsertAsync(watchedAnime);

                await _unitOfWork.WatchedAnimeRepo.Delete(anime);

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in MoveToCurrentlyWatchingFromAlreadyWatched -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToAlreadyWatchedFromWatchList(MoveAnimeWithinUserLists request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new MoveAnimeWithinUserListsValidator().Validate(request);

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

                ToWatchAnimes? anime = await _unitOfWork.ToWatchAnimeRepo.GetById(request.AnimeId);

                if (anime == null)
                {
                    List<string> errors = new()
                    {
                        "Anime Cannot Be Found"
                    };
                    return response.BuildFailureResponse(errors);
                }

                if (anime.UserId != request.UserId)
                {
                    List<string> errors = new()
                    {
                        "Anime cannot be found in User List"
                    };
                    return response.BuildFailureResponse(errors);
                }

                WatchedAnimes watchedAnime = WatchedAnimes.Create(anime);

                await _unitOfWork.WatchedAnimeRepo.InsertAsync(watchedAnime);

                await _unitOfWork.ToWatchAnimeRepo.Delete(anime);

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in MoveToAlreadyWatchedFromWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToAlreadyWatchedFromCurrentlyWatching(MoveAnimeWithinUserLists request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new MoveAnimeWithinUserListsValidator().Validate(request);

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

                WatchingAnimes? anime = await _unitOfWork.WatchingAnimeRepo.GetById(request.AnimeId);

                if (anime == null)
                {
                    List<string> errors = new()
                    {
                        "Anime Cannot Be Found"
                    };
                    return response.BuildFailureResponse(errors);
                }

                if (anime.UserId != request.UserId)
                {
                    List<string> errors = new()
                    {
                        "Anime cannot be found in User List"
                    };
                    return response.BuildFailureResponse(errors);
                }

                WatchedAnimes watchedAnime = WatchedAnimes.Create(anime);

                await _unitOfWork.WatchedAnimeRepo.InsertAsync(watchedAnime);

                await _unitOfWork.WatchingAnimeRepo.Delete(anime);

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in MoveToAlreadyWatchedFromCurrentlyWatching -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToWatchListFromAlreadyWatched(MoveAnimeWithinUserLists request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new MoveAnimeWithinUserListsValidator().Validate(request);

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

                WatchedAnimes? anime = await _unitOfWork.WatchedAnimeRepo.GetById(request.AnimeId);

                if (anime == null)
                {
                    List<string> errors = new()
                    {
                        "Anime Cannot Be Found"
                    };
                    return response.BuildFailureResponse(errors);
                }

                if (anime.UserId != request.UserId)
                {
                    List<string> errors = new()
                    {
                        "Anime cannot be found in User List"
                    };
                    return response.BuildFailureResponse(errors);
                }

                ToWatchAnimes watchedAnime = ToWatchAnimes.Create(anime);

                await _unitOfWork.ToWatchAnimeRepo.InsertAsync(watchedAnime);

                await _unitOfWork.WatchedAnimeRepo.Delete(anime);

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in MoveToWatchListFromAlreadyWatched -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }
    
        public async Task<GeneralResponseWrapper<bool>> MoveToWatchListFromCurrentlyWatching(MoveAnimeWithinUserLists request)
        {
            var response = new GeneralResponseWrapper<bool>();

            var validator = new MoveAnimeWithinUserListsValidator().Validate(request);

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

                WatchingAnimes? anime = await _unitOfWork.WatchingAnimeRepo.GetById(request.AnimeId);

                if (anime == null)
                {
                    List<string> errors = new()
                    {
                        "Anime Cannot Be Found"
                    };
                    return response.BuildFailureResponse(errors);
                }

                if (anime.UserId != request.UserId)
                {
                    List<string> errors = new()
                    {
                        "Anime cannot be found in User List"
                    };
                    return response.BuildFailureResponse(errors);
                }

                ToWatchAnimes watchedAnime = ToWatchAnimes.Create(anime);

                await _unitOfWork.ToWatchAnimeRepo.InsertAsync(watchedAnime);

                await _unitOfWork.WatchingAnimeRepo.Delete(anime);

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in MoveToWatchListFromCurrentlyWatching -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }
    
               
    
    }
}

