namespace MyDemoProjects.Client.Services.AnimeList
{
    public interface IAnimeList
    {
        IEnumerable<Datum> AnimeLists { get; set; }
        Task GetAnimeListBySeasonAndYear(Season season);
    }
}
