using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Application.Features.AnimeList.Queries;
using MyDemoProjects.Application.Shared.DTO.Response;

namespace MyDemoProjects.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AnimeListController : Controller
{
    private readonly ISender _mediator;
    
    public AnimeListController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("api/animeList")]
    public async Task<ApplicationResponse<AnimeListRoot>> GetAnimeList(Season season)
    {
        return await _mediator.Send(new GetAnimeListBySeasonAndYear(season));
    }
}
