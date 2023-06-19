using Amazon.Runtime.Internal;
using FluentValidation;
using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence;
using HisashiburiDana.Application.Validators;
using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Contract.Users;
using HisashiburiDana.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using MapsterMapper;

namespace HisashiburiDana.Application.Services
{
    public class UserAnimeService : IUserAnimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserAnimeService> _logger;
        private readonly IMapper _mapper;

        public UserAnimeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserAnimeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GeneralResponseWrapper<bool>> AddNewAnimeToWatchList(AddAnimeFromAniList request)
        {
            _logger.LogInformation($"AddNewAnimeToWatchList Request Arrived ---{JsonConvert.SerializeObject(request)}");
            var response = new GeneralResponseWrapper<bool>(_logger);

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
                _logger.LogInformation($"Created AddToWatchListEntity Successfully for UserId ---{request.UserId}");
                foreach (var ranking in request.Media.Rankings)
                {
                    var rank = Rankings.Create(ranking, request.UserId);
                    await _unitOfWork.AnimeRankingsRepo.InsertAsync(rank);
                    rankiIds.Add(rank.Id);
                }
                var addtoWatchlistentitywithRanks = ToWatchAnimes.AddRankingId(addtoWatchlistentity, rankiIds);
                _logger.LogInformation($"Added RankingIds Successfully for UserId ---{request.UserId}");
                await _unitOfWork.ToWatchAnimeRepo.InsertAsync(addtoWatchlistentitywithRanks);
                return response.BuildSuccessResponse(true);
                 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in AddNewAnimeWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
                 
            }
        }
    
        public async Task<GeneralResponseWrapper<bool>> AddNewAnimeToAlreadyWatched(AddAnimeFromAniList request)
        {
            _logger.LogInformation($"AddNewAnimeToAlready Watched Request Arrived ---{JsonConvert.SerializeObject(request)}");
            var response = new GeneralResponseWrapper<bool>(_logger);

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
                _logger.LogInformation($"Created addToAlreadyWatched Successfully for UserId ---{request.UserId}");

                List<string> rankiIds = new();
                foreach (var ranking in request.Media.Rankings)
                {
                    var rank = Rankings.Create(ranking, request.UserId);
                    await _unitOfWork.AnimeRankingsRepo.InsertAsync(rank);
                    rankiIds.Add(rank.Id);
                }
                var addtowatchedwithRanks = WatchedAnimes.AddRankingId(addToAlreadyWatched, rankiIds);
                _logger.LogInformation($"Added RankingIds Successfully for UserId ---{request.UserId}");
                await _unitOfWork.WatchedAnimeRepo.InsertAsync(addtowatchedwithRanks);
                return response.BuildSuccessResponse(true);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in AddNewAnimeToAlreadyWatched -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });

            }



        }
        
        public async Task<GeneralResponseWrapper<bool>> AddNewAnimeToCurrentlyWatching(AddAnimeFromAniList request)
        {
            _logger.LogInformation($"AddNewAnimeToCurrentlyWatching Watched Request Arrived ---{JsonConvert.SerializeObject(request)}");

            var response = new GeneralResponseWrapper<bool>(_logger);

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
                _logger.LogInformation($"Created addToCurrentlyWatching Successfully for UserId ---{request.UserId}");

                List<string> rankiIds = new();
                foreach (var ranking in request.Media.Rankings)
                {
                    var rank = Rankings.Create(ranking, request.UserId);
                    await _unitOfWork.AnimeRankingsRepo.InsertAsync(rank);
                    rankiIds.Add(rank.Id);
                }
                var addtowatchingwithRanks = WatchingAnimes.AddRankingId(addToCurrentlyWatching, rankiIds);
                _logger.LogInformation($"Added RankingIds Successfully for UserId ---{request.UserId}");

                await _unitOfWork.WatchingAnimeRepo.InsertAsync(addtowatchingwithRanks);
                return response.BuildSuccessResponse(true);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in AddNewAnimeToCurrentlyWatching -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });

            }
        }
    
        public async Task<GeneralResponseWrapper<bool>> MoveToCurrentlyWatchingFromWatchList(MoveAnimeWithinUserLists request)
        {
            _logger.LogInformation($"MoveToCurrentlyWatchingFromWatchList Request Arrived ---{JsonConvert.SerializeObject(request)}");
            var response = new GeneralResponseWrapper<bool>(_logger);

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
                _logger.LogInformation($"Created and Inserted watchingAnime Successfully for UserId ---{request.UserId}");

                await _unitOfWork.ToWatchAnimeRepo.Delete(anime);
                _logger.LogInformation($"Deleted Anime from Watchlist Successfully for UserId ---{request.UserId}");

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in MoveToCurrentlyWatchingFromWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToCurrentlyWatchingFromAlreadyWatched(MoveAnimeWithinUserLists request)
        {
            _logger.LogInformation($"MoveToCurrentlyWatchingFromAlreadyWatched Request Arrived ---{JsonConvert.SerializeObject(request)}");
            var response = new GeneralResponseWrapper<bool>(_logger);

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
                _logger.LogInformation($"Created and Inserted watchedAnime Successfully for UserId ---{request.UserId}");

                await _unitOfWork.WatchedAnimeRepo.Delete(anime);
                _logger.LogInformation($"Deleted Anime from Already Watched Successfully for UserId ---{request.UserId}");

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in MoveToCurrentlyWatchingFromAlreadyWatched -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToAlreadyWatchedFromWatchList(MoveAnimeWithinUserLists request)
        {
            _logger.LogInformation($"MoveToAlreadyWatchedFromWatchList Request Arrived ---{JsonConvert.SerializeObject(request)}");

            var response = new GeneralResponseWrapper<bool>(_logger);

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
                _logger.LogInformation($"Created and Inserted watchedAnime Successfully for UserId ---{request.UserId}");

                await _unitOfWork.ToWatchAnimeRepo.Delete(anime);
                _logger.LogInformation($"Deleted Anime from WatchList Successfully for UserId ---{request.UserId}");


                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in MoveToAlreadyWatchedFromWatchList -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToAlreadyWatchedFromCurrentlyWatching(MoveAnimeWithinUserLists request)
        {
            _logger.LogInformation($"MoveToAlreadyWatchedFromCurrentlyWatching Request Arrived ---{JsonConvert.SerializeObject(request)}");

            var response = new GeneralResponseWrapper<bool>(_logger);

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
                _logger.LogInformation($"Created and Inserted watchedAnime Successfully for UserId ---{request.UserId}");

                await _unitOfWork.WatchingAnimeRepo.Delete(anime);
                _logger.LogInformation($"Deleted Anime from Currently Watching Successfully for UserId ---{request.UserId}");


                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in MoveToAlreadyWatchedFromCurrentlyWatching -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<bool>> MoveToWatchListFromAlreadyWatched(MoveAnimeWithinUserLists request)
        {
            _logger.LogInformation($"MoveToWatchListFromAlreadyWatched Request Arrived ---{JsonConvert.SerializeObject(request)}");

            var response = new GeneralResponseWrapper<bool>(_logger);

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

                ToWatchAnimes toWatchdAnime = ToWatchAnimes.Create(anime);

                await _unitOfWork.ToWatchAnimeRepo.InsertAsync(toWatchdAnime);
                _logger.LogInformation($"Created and Inserted toWatchdAnime Successfully for UserId ---{request.UserId}");


                await _unitOfWork.WatchedAnimeRepo.Delete(anime);
                _logger.LogInformation($"Deleted Anime from Already Watched Successfully for UserId ---{request.UserId}");

                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in MoveToWatchListFromAlreadyWatched -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }
    
        public async Task<GeneralResponseWrapper<bool>> MoveToWatchListFromCurrentlyWatching(MoveAnimeWithinUserLists request)
        {
            _logger.LogInformation($" MoveToWatchListFromCurrentlyWatching Request Arrived ---{JsonConvert.SerializeObject(request)}");

            var response = new GeneralResponseWrapper<bool>(_logger);

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

                ToWatchAnimes towatchAnime = ToWatchAnimes.Create(anime);

                await _unitOfWork.ToWatchAnimeRepo.InsertAsync(towatchAnime);
                _logger.LogInformation($"Created and Inserted toWatchdAnime Successfully for UserId ---{request.UserId}");

                await _unitOfWork.WatchingAnimeRepo.Delete(anime);
                _logger.LogInformation($"Deleted Anime from WatchList Successfully for UserId ---{request.UserId}");


                return response.BuildSuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred in MoveToWatchListFromCurrentlyWatching -- {ex.Message}--\n {ex.Message}");
                return response.BuildFailureResponse(new List<string> { "Error Occurred While Adding to List" });
            }
        }

        public async Task<GeneralResponseWrapper<GetUserAnimesResponse>> GetUserAnimes(string userId)
        {
            _logger.LogInformation($"GetUserAnimes Request Arrived ---{userId}");

            var response = new GeneralResponseWrapper<GetUserAnimesResponse>(_logger);

            User? user = await _unitOfWork.UserRepo.Get("Id", ScanOperator.Equal, userId);
            if (user == null)
            {
                List<string> errors = new()
                {
                    "User Does Not Exist"
                };
                return response.BuildFailureResponse(errors);
            };

            
            List<WatchedAnimes> userWatchedAnimes = await _unitOfWork.WatchedAnimeRepo.GetAll("UserId", ScanOperator.Equal, userId);
           
            List<WatchingAnimes> userWatchingAnimes = await _unitOfWork.WatchingAnimeRepo.GetAll("UserId", ScanOperator.Equal, userId);
            
            List<ToWatchAnimes> userToWatchAnimes = await _unitOfWork.ToWatchAnimeRepo.GetAll("UserId", ScanOperator.Equal, userId);

            _logger.LogInformation($"Found List of all User Animes ---{userId}");
            var animeCollection = new GetUserAnimesResponse();

            foreach (var anime in userWatchedAnimes)
            {
                var userAnime =  _mapper.Map(anime, new UserAnime());
                animeCollection.Watched.Add(userAnime);                
            }
            _logger.LogInformation($"Added User Watched Animes to Collection");

            foreach (var anime in userWatchingAnimes)
            {
                var userAnime = _mapper.Map(anime, new UserAnime());
                animeCollection.Watching.Add(userAnime);
            }
            _logger.LogInformation($"Added User Watching Animes to Collection");


            foreach (var anime in userToWatchAnimes)
            {
                var userAnime = _mapper.Map(anime, new UserAnime());
                animeCollection.WatchList.Add(userAnime);
            }
            _logger.LogInformation($"Added User To Watch Animes to Collection");


            return response.BuildSuccessResponse(animeCollection);
        }
    }
}

