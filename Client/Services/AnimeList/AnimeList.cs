using MyDemoProjects.Client.Pages.MyAnimeList;
using System.Net.Http.Json;

namespace MyDemoProjects.Client.Services.AnimeList
{
    public class AnimeList : IAnimeList
    {
        private readonly HttpClient _httpClient;

        public AnimeList(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<Datum> AnimeLists { get; set; }

        public async Task GetAnimeListBySeasonAndYear(Season season)
        {

            var response = await _httpClient.PostAsJsonAsync("/AnimeList/api/getAnimeListBySeasonAndYear", season);
            var result = response.Content
                .ReadFromJsonAsync<ServerResponse<AnimeListRoot>>().Result;

            AnimeLists = result.Data.Data.ToList();


        }
    }
}
