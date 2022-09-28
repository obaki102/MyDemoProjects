using MediatR;
using MyDemoProjects.Application.Features.AnimeList.Queries;
using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.AnimeList
{
    public class AnimeList : IAnimeList
    {
        private readonly ISender _mediator;

        public AnimeList( ISender  mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<Datum> AnimeLists { get; set; }

        public async Task GetAnimeListBySeasonAndYear(Season season)
        {

            var result = await _mediator.Send(new GetAnimeListBySeasonAndYear(season));

            AnimeLists = result.Data.Data;


        }
    }
}
