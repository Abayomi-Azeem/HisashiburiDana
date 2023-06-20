using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Authentication;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Contract.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HisashiburiDana.Api.Controllers
{

    /// <summary>
    /// Handles User-Anime Operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserAnimeService _userService;

        public UserController(IUserAnimeService userService)
        {
            _userService = userService;
        }



        /// <summary>
        /// Get List of User Watchlist, Currently Watching Animes and Already Watched Animes
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getuseranimelist")]
        [ProducesResponseType(typeof(GeneralResponseWrapper<GetUserAnimesResponse>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnimes(string UserId)
        {
            var response = await _userService.GetUserAnimes(UserId);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Add New Anime to User Watchlist
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("addtowatchlist")]
        public async Task<IActionResult> AddToWatchlist([FromBody] AddAnimeFromAniList animeDetails)
        {
            var response = await _userService.AddNewAnimeToWatchList(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Add New Anime to User Already Watched List
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addnewtoalreadywatched")]
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        public async Task<IActionResult> AddToAlreadyWatched([FromBody] AddAnimeFromAniList animeDetails)
        {
            var response = await _userService.AddNewAnimeToAlreadyWatched(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Add New Anime to User Currently Watching List
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("addnewtocurrentlywatching")]
        public async Task<IActionResult> AddToCurrentlyWatching([FromBody] AddAnimeFromAniList animeDetails)
        {
            var response = await _userService.AddNewAnimeToCurrentlyWatching(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Move Anime To User Currently Watching List From User WatchList
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("movetowatchingfromwatchlist")]
        public async Task<IActionResult> MoveToCurrentlyWatchingFromToWatch([FromBody] MoveAnimeWithinUserLists animeDetails)
        {
            var response = await _userService.MoveToCurrentlyWatchingFromWatchList(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Move Anime To User Currently Watching List From Already Watched List
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("movetowatchingfromwatched")]
        public async Task<IActionResult> MoveToCurrentlyWatchingFromWatched([FromBody] MoveAnimeWithinUserLists animeDetails)
        {
            var response = await _userService.MoveToCurrentlyWatchingFromAlreadyWatched(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Move Anime To User Already Watched List From Currently Watching List
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("movetowatchedfromwatching")]
        public async Task<IActionResult> MoveToWatchedFromWatching([FromBody] MoveAnimeWithinUserLists animeDetails)
        {
            var response = await _userService.MoveToAlreadyWatchedFromCurrentlyWatching(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Move Anime To User Already Watched List From User Watch List
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("movetowatchedfromwatchlist")]
        public async Task<IActionResult> MoveToWatchedFromWatchlist([FromBody] MoveAnimeWithinUserLists animeDetails)
        {
            var response = await _userService.MoveToAlreadyWatchedFromWatchList(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Move Anime To User WatchList From User Already Watched List
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("movetowatchlistfromwatched")]
        public async Task<IActionResult> MoveToWatchListdFromWatched([FromBody] MoveAnimeWithinUserLists animeDetails)
        {
            var response = await _userService.MoveToWatchListFromAlreadyWatched(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Move Anime To User WatchList From User Currently Watching List
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("movetowatchlistfromwatching")]
        public async Task<IActionResult> MoveToWatchListdFromWatching([FromBody] MoveAnimeWithinUserLists animeDetails)
        {
            var response = await _userService.MoveToWatchListFromCurrentlyWatching(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Delete Anime From Watchlist
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("deletefromwatchlist")]
        public async Task<IActionResult> DeleteFromWatchlist([FromBody] DeleteAnimeRequest animeDetails)
        {
            var response = await _userService.DeleteAnimeFromWatchList(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Delete Anime From Already Watched
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("deletefromalreadywatched")]
        public async Task<IActionResult> DeletefromAlreadywatched([FromBody] DeleteAnimeRequest animeDetails)
        {
            var response = await _userService.DeleteAnimeFromAlreadyWatched(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        /// <summary>
        /// Delete Anime From Currently Watching
        /// </summary>
        /// <param name="animeDetails"></param>
        /// <returns></returns>
        [Produces(typeof(GeneralResponseWrapper<bool>))]
        [HttpPost]
        [Route("deletefromcurrentlywatching")]
        public async Task<IActionResult> DeleteFromcurrentlywatching([FromBody] DeleteAnimeRequest animeDetails)
        {
            var response = await _userService.DeleteAnimeFromCurrentlyWatching(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
