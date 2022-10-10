namespace MyDemoProjects.Application.Features.Shared.Service.Http.AnimeList
{
    public class AnimeListHttpService : IAnimeListHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IJsonStreamSerializer _jsonSerializer;
        private readonly IConfiguration _configuration;

        public AnimeListHttpService(HttpClient httpClient, IJsonStreamSerializer jsonSerializer, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri("https://api.myanimelist.net/");
            _httpClient.DefaultRequestHeaders.Add(AppSecrets.AnimeList.XmalClientId, _configuration.GetSection(AppSecrets.AnimeList.AnimelistClientId).Value);

        }

        public async Task<ApplicationResponse<AnimeListRoot>> GetAnimeListBySeasonAndYear(Season season)
        {
            var uriRequest = $"v2/anime/season/{season.Year}/{season.SeasonOfTheYear}?limit=100&fields=id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics";
            var response = await _httpClient.GetAsync(uriRequest);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStreamAsync();
                var data = _jsonSerializer.DeserializeStream<AnimeListRoot>(result);
                return ApplicationResponse<AnimeListRoot>.Success(data);
            }
            return ApplicationResponse<AnimeListRoot>.Fail(response.StatusCode.ToString());
        }
    }
}
