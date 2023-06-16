using Azure.Core;
using HisashiburiDana.Application.Abstractions.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HisashiburiDana.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AniListController : ControllerBase
    {
        private readonly IAniListService _anilistService;

        public AniListController(IAniListService anilistService)
        {
            _anilistService = anilistService;
        }


        [Route("getanimes")]
        [HttpGet]
        public async Task<IActionResult> GetAnimes(int PageNumber = 1)
        {
            var response = await _anilistService.GetAnimes(PageNumber);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        } 

        [Route("getgenres")]
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var response = await _anilistService.GetAllGenres();
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Route("gettrendinganimes")]
        [HttpGet]
        public async Task<IActionResult> GetTrending()
        {
            var response = await _anilistService.GetTrendingAnime();
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Route("searchanime")]
        [HttpPost]
        public async Task<IActionResult> SearchAnime(string animeName, int pageNumber = 1)
        {
            var response = await _anilistService.SearchInAnimeList(animeName,pageNumber);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    
        //Sort by Title, No of Episodes, etc

        //Filter by Genre
    }
}
