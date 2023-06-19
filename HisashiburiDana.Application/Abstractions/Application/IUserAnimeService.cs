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
        Task<GeneralResponseWrapper<bool>> AddNewAnimeToWatchList(AddAnimeFromAniList request);

        Task<GeneralResponseWrapper<bool>> AddNewAnimeToAlreadyWatched(AddAnimeFromAniList request);

        Task<GeneralResponseWrapper<bool>> AddNewAnimeToCurrentlyWatching(AddAnimeFromAniList request);

        Task<GeneralResponseWrapper<bool>> MoveToCurrentlyWatchingFromWatchList(MoveAnimeWithinUserLists request);

        Task<GeneralResponseWrapper<bool>> MoveToCurrentlyWatchingFromAlreadyWatched(MoveAnimeWithinUserLists request);

        Task<GeneralResponseWrapper<bool>> MoveToAlreadyWatchedFromWatchList(MoveAnimeWithinUserLists request);

        Task<GeneralResponseWrapper<bool>> MoveToAlreadyWatchedFromCurrentlyWatching(MoveAnimeWithinUserLists request);

        Task<GeneralResponseWrapper<bool>> MoveToWatchListFromAlreadyWatched(MoveAnimeWithinUserLists request);

        Task<GeneralResponseWrapper<bool>> MoveToWatchListFromCurrentlyWatching(MoveAnimeWithinUserLists request);
        Task<GeneralResponseWrapper<GetUserAnimesResponse>> GetUserAnimes(string userId);
        Task<GeneralResponseWrapper<bool>> DeleteAnimeFromWatchList(DeleteAnimeRequest request);
        Task<GeneralResponseWrapper<bool>> DeleteAnimeFromAlreadyWatched(DeleteAnimeRequest request);
        Task<GeneralResponseWrapper<bool>> DeleteAnimeFromCurrentlyWatching(DeleteAnimeRequest request);
    }
}
