namespace MyDemoProjects.Application.Features.Shared.Service.Http.RandomGOTQuotes
{
    public class RandomGotQuotesHttpService : IRandomGotQuotesHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IJsonStreamSerializer _jsonSerializer;

        public RandomGotQuotesHttpService(HttpClient httpClient, IJsonStreamSerializer jsonSerializer)
        {
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
            _httpClient.BaseAddress = new Uri("https://api.gameofthronesquotes.xyz/");
        }
        public async Task<ApplicationResponse<RandomGOTQuotesResponse>> GetRandomQuotes()
        {
            var response = await _httpClient.GetAsync("v1/random");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStreamAsync();
                var data = _jsonSerializer.DeserializeStream<RandomGOTQuotesResponse>(result);
                return ApplicationResponse<RandomGOTQuotesResponse>.Success(data);
            }
            return ApplicationResponse<RandomGOTQuotesResponse>.Fail(response.StatusCode.ToString());
        }
    }
}
