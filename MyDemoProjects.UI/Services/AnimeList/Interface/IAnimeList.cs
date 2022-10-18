using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.AnimeList.Interface
{
    public interface IAnimeList
    {
        IEnumerable<Datum> AnimeLists { get; set; }
        Task<IEnumerable<Datum>> GetAnimeListBySeasonAndYear(Season season);
    }
}
