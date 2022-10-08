﻿using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Application.Features.ExternalApi.AnimeList.Queries;

public record GetAnimeListBySeasonAndYear(Season Season) : IRequest<ApplicationResponse<AnimeListRoot>>;

public class GetAnimeListBySeasonAndYearHandler : IRequestHandler<GetAnimeListBySeasonAndYear, ApplicationResponse<AnimeListRoot>>
{
    private readonly IHttpService _httpService;
    private readonly IJsonStreamSerializer _jsonSerializer;

    public GetAnimeListBySeasonAndYearHandler(IHttpService httpService, IJsonStreamSerializer jsonSerializer)
    {
        _httpService = httpService;
        _jsonSerializer = jsonSerializer;
    }
    public async Task<ApplicationResponse<AnimeListRoot>> Handle(GetAnimeListBySeasonAndYear request, CancellationToken cancellationToken)
    {

        var options = new HttpServiceOption
        {
            IsTokenRequired = false,
            IsAnimeList = true,
            Endpoint = new Uri($"https://api.myanimelist.net/v2/anime/season/{request.Season.Year}/{request.Season.SeasonOfTheYear}?limit=100&fields=id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics")

        };

        var response = await _httpService.GetResponse(options);
        var result = await response.Content.ReadAsStreamAsync();
        var data = _jsonSerializer.DeserializeStream<AnimeListRoot>(result);
        return ApplicationResponse<AnimeListRoot>.Success(data);
        
    }
}