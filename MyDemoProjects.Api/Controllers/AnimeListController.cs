using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Application.Features.AnimeList.Queries;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;

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
    public async Task<ActionResult<ApplicationResponse<AnimeListRoot>>> GetAnimeList(Season season)
    {
        return Ok(await _mediator.Send(new GetAnimeListBySeasonAndYear(season)));
    }
}
