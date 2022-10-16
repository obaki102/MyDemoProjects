using MediatR;
using Microsoft.AspNetCore.Authorization;
using MyDemoProjects.Application.Features.ExternalApi.RandomGOTQuotes.Queries;
using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.RandomGOTQuotes
{
    [Authorize]
    public class RandomGOTQuotes : IRandomGOTQuotes
    {
        private readonly ISender _mediator;

        public RandomGOTQuotes(ISender mediator)
        {
            _mediator = mediator;
        }

        public RandomGOTQuotesResponse GOTQuotes { get; set; } = default;

        public async Task<RandomGOTQuotesResponse> GetRandomGOTQuotes()
        {
            var randomGOTQuotes = await _mediator.Send(new GetRandomQuotes());
            if (randomGOTQuotes.Data is not null)
            {
                GOTQuotes = randomGOTQuotes.Data;
            }

            return GOTQuotes;
        }
    }
}
