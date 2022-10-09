
using MyDemoProjects.Application.Features.Shared.Service.Http.RandomGOTQuotes;

namespace MyDemoProjects.Application.Features.ExternalApi.RandomGOTQuotes.Queries
{

    public record GetRandomQuotes() : IRequest<ApplicationResponse<RandomGOTQuotesResponse>>;

    public class GetRandomQuotesHandler : IRequestHandler<GetRandomQuotes, ApplicationResponse<RandomGOTQuotesResponse>>
    {
        private readonly IRandomGotQuotesHttpService _randomGotQuotesHttpService;
        public GetRandomQuotesHandler(IRandomGotQuotesHttpService randomGotQuotesHttpService)
        {
            _randomGotQuotesHttpService = randomGotQuotesHttpService;
        }

        public async Task<ApplicationResponse<RandomGOTQuotesResponse>> Handle(GetRandomQuotes request, CancellationToken cancellationToken)
        {
            return await _randomGotQuotesHttpService.GetRandomQuotes();
        }
    }
}
