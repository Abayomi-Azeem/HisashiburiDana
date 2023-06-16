using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Authentication;
using HisashiburiDana.Contract.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HisashiburiDana.Api.Controllers
{
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


        //addToWAtchlist
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

        [HttpPost]
        [Route("addnewtoalreadywatched")]
        public async Task<IActionResult> AddToAlreadyWatched([FromBody] AddAnimeFromAniList animeDetails)
        {
            var response = await _userService.AddNewAnimeToAlreadyWatched(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

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


        
        //deleteFromWatchilist

        //deletefromAlreadywatched

        //deleteFromcurrentlywatching
    }
}
