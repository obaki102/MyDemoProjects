using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Application.Features.RandomGOTQuotes.Queries;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GetRandomQuotesController : Controller
{
    private readonly ISender _mediator;
	public GetRandomQuotesController(ISender mediator)
	{
		_mediator = mediator;
	}

    [HttpPost("api/gotrandomquotes")]
    public async Task<ActionResult<ApplicationResponse<RandomGOTQuotesResponse>>> GetGOTRandomQuotes()
	{
		return await _mediator.Send(new GetRandomQuotes());

    }
}
