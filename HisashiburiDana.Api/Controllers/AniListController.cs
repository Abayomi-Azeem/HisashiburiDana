using Azure.Core;
using HisashiburiDana.Application.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HisashiburiDana.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AniListController : ControllerBase
    {
        private readonly IAniListService _anilistService;

        public AniListController(IAniListService anilistService)
        {
            _anilistService = anilistService;
        }


        [Route("getanimes")]
        [HttpGet]
        public async Task<IActionResult> GetAnimes(int PageNumber)
        {
            var response = await _anilistService.GetAnimes(PageNumber);
            if (response.HasError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
