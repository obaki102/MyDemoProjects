
namespace MyDemoProjects.Application.Features.AnimeList.Commands;
public record GetToken() : IRequest<ApplicationResponse<TokenResponse>>;

public class GetTokenhandler : IRequestHandler<GetToken, ApplicationResponse<TokenResponse>>
{
    private readonly IHttpService _httpService;
    public GetTokenhandler(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<ApplicationResponse<TokenResponse>> Handle(GetToken request, CancellationToken cancellationToken)
    {

        var options = new HttpServiceOption
        {
            Endpoint = new Uri("https://api.myanimelist.net/v2/anime/season/2022/fall?limit=100&fields=id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics")
        };

        var response = await _httpService.GetResponse(options);
        var content = await response.Content.ReadAsStreamAsync();
        return null;

    }
}



