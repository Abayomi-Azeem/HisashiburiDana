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
        public async Task<IActionResult> AddToWatchlist([FromBody]AddAnimeToWatchListRequest animeDetails)
        {
            var response = await _userService.AddNewAnimeToWatchList(animeDetails);

            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        //addtoalreadywatched

        //addToCurrentlywatching

        //moveToCurrentlywatching

        //moveToalredyWatched

        //deleteFromWatchilist

        //deletefromAlreadywatched

        //deleteFromcurrentlywatching
    }
}
