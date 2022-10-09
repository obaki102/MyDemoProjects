using MyDemoProjects.Application.Features.Shared.Service.Http.AnimeList;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Application.Features.ExternalApi.AnimeList.Queries;

public record GetAnimeListBySeasonAndYear(Season Season) : IRequest<ApplicationResponse<AnimeListRoot>>;

public class GetAnimeListBySeasonAndYearHandler : IRequestHandler<GetAnimeListBySeasonAndYear, ApplicationResponse<AnimeListRoot>>
{
    private readonly IAnimeListHttpService _animeListHttpService;
    public GetAnimeListBySeasonAndYearHandler(IAnimeListHttpService animeListHttpService)
    {
        _animeListHttpService = animeListHttpService;
    }
    public async Task<ApplicationResponse<AnimeListRoot>> Handle(GetAnimeListBySeasonAndYear request, CancellationToken cancellationToken)
    {
        return await _animeListHttpService.GetAnimeListBySeasonAndYear(request.Season);


    }
}