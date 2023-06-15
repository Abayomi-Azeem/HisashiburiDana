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
        [Route("movetocurrentlywatchingfromwatchlist")]
        public async Task<IActionResult> MoveToCurrentlyWatching([FromBody] MoveAnimeWithinUserLists animeDetails)
        {
            var response = await _userService.MoveToCurrentlyWatchingFromWatchList(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //moveToCurrentlywatching

        //moveToalredyWatched

        //deleteFromWatchilist

        //deletefromAlreadywatched

        //deleteFromcurrentlywatching
    }
}
