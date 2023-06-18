using Azure.Core;
using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Enums;
using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace HisashiburiDana.Api.Controllers
{
    /// <summary>
    /// Handles Anime Listing Operations
    /// </summary>
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


        /// <summary>
        /// Get List of Animes Page by Page
        /// </summary>
        /// <param name="PageNumber"></param> The Page you  want to Search For
        /// <returns></returns>
        [Route("getanimes")]
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseWrapper<AnimeList>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseWrapper<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAnimes(int PageNumber = 1)
        {
            var response = await _anilistService.GetAnimes(PageNumber);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        } 


        /// <summary>
        /// Get All Available Anime Genres
        /// </summary>
        /// <returns></returns>
        [Route("getgenres")]
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseWrapper<AllGenres>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseWrapper<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGenres()
        {
            var response = await _anilistService.GetAllGenres();
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }



        /// <summary>
        /// Get Top 10 Trending Animes
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(GeneralResponseWrapper<AnimeList>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseWrapper<>), StatusCodes.Status400BadRequest)]
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



        /// <summary>
        /// Get Animes by Title: Either English or Romanji
        /// </summary>
        /// <param name="animeName"></param> The Name you want to search by
        /// <returns></returns>
        [Route("searchanime")]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseWrapper<AnimeList>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseWrapper<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchAnime(string animeName)
        {
            var response = await _anilistService.SearchInAnimeList(animeName);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }




        /// <summary>
        /// Sort Animes by SortBy Property
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("getsortedanimes")]
        [HttpGet]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseWrapper<AnimeList>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseWrapper<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SortAnime(Sorter sortBy, int pageNumber)
        {
            var response = await _anilistService.GetSortedAnimes(sortBy, pageNumber);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //Sort by Title, No of Episodes, etc

        //Filter by Genre, episodes greater than,episodes lesser than, status
        [Route("filteranime")]
        [HttpPost]
        public async Task<IActionResult> FilterAnime([FromBody] FilterRequest payload)
        {
            var response = await _anilistService.FilterAnime(payload);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
