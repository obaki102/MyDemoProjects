using MediatR;
using MyDemoProjects.Application.Features.RandomGOTQuotes.Queries;
using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.RandomGOTQuotes
{
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
            GOTQuotes = randomGOTQuotes.Data;
            return randomGOTQuotes.Data;
        }
    }
}
