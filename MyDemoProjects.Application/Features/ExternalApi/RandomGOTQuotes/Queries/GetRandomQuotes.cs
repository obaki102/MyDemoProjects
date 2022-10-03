
namespace MyDemoProjects.Application.Features.ExternalApi.RandomGOTQuotes.Queries
{

    public record GetRandomQuotes() : IRequest<ApplicationResponse<RandomGOTQuotesResponse>>;

    public class GetRandomQuotesHandler : IRequestHandler<GetRandomQuotes, ApplicationResponse<RandomGOTQuotesResponse>>
    {
        private readonly IHttpService _httpService;
        private readonly IJsonStreamSerializer _jsonSerializer;
        public GetRandomQuotesHandler(IHttpService httpService, IJsonStreamSerializer jsonSerializer)
        {
            _httpService = httpService;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<ApplicationResponse<RandomGOTQuotesResponse>> Handle(GetRandomQuotes request, CancellationToken cancellationToken)
        {
            var options = new HttpServiceOption
            {
                IsTokenRequired = false,
                Endpoint = new Uri("https://api.gameofthronesquotes.xyz/v1/random")

            };

            var randomQuotes = await _httpService.GetResponse(options);
            var randomQuotesStream = await randomQuotes.Content.ReadAsStreamAsync();
            var randomQuotesSerialized = _jsonSerializer.DeserializeStream<RandomGOTQuotesResponse>(randomQuotesStream);
            return ApplicationResponse<RandomGOTQuotesResponse>.Success(randomQuotesSerialized);
        }
    }
}
