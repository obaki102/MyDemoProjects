using Microsoft.Extensions.Caching.Memory;
using MyDemoProjects.Application.Features.Shared.Service.Http.AnimeList;

namespace MyDemoProjects.Application.Features.ExternalApi.AnimeList.Queries;

public record GetAnimeListBySeasonAndYear(Season Season) : IRequest<ApplicationResponse<AnimeListRoot>>;

public class GetAnimeListBySeasonAndYearHandler : IRequestHandler<GetAnimeListBySeasonAndYear, ApplicationResponse<AnimeListRoot>>
{
    private readonly IAnimeListHttpService _animeListHttpService;
    private readonly IMemoryCache _memoryCache;
    public GetAnimeListBySeasonAndYearHandler(IAnimeListHttpService animeListHttpService, IMemoryCache memoryCache)
    {
        _animeListHttpService = animeListHttpService;
        _memoryCache = memoryCache;
    }
    public  Task<ApplicationResponse<AnimeListRoot>> Handle(GetAnimeListBySeasonAndYear request, CancellationToken cancellationToken)
    {
        var memoryKey = new Guid();
        return  _memoryCache.GetOrCreateAsync(
            memoryKey,
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
                return await _animeListHttpService.GetAnimeListBySeasonAndYear(request.Season);
            });

    }
}