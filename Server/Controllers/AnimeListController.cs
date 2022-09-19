using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Server.Application.Features.AnimeList.Queries;

namespace MyDemoProjects.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AnimeListController : Controller
    {

        private readonly ISender _mediator;
        public AnimeListController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/getAnimeListBySeasonAndYear")]
        public async Task<ActionResult<ServerResponse<AnimeListRoot>>> GetAnimeListBySeasonAndYear(Season season)
        {
            return await _mediator.Send(new GetAnimeListBySeasonAndYear(season));
        }
    }
}
