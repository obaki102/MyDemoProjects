using MediatR;
using MyDemoProjects.Application.Features.ExternalApi.AnimeList.Queries;
using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.AnimeList
{
    public class AnimeList : IAnimeList
    {
        private readonly ISender _mediator;
        public AnimeList(ISender  mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<Datum> AnimeLists { get; set; } = default;
        public async Task<IEnumerable<Datum>> GetAnimeListBySeasonAndYear(Season season)
        {
            var animeListResult = await _mediator.Send(new GetAnimeListBySeasonAndYear(season));
            if(animeListResult.Data is not  null && animeListResult.Data.Data is not null)
            {
                AnimeLists = animeListResult.Data.Data;
            }
            
            return AnimeLists;
        }
    }
}

