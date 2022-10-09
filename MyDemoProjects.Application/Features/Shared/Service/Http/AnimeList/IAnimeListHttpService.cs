using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Features.Shared.Service.Http.AnimeList
{
    public interface IAnimeListHttpService
    {
        Task<ApplicationResponse<AnimeListRoot>> GetAnimeListBySeasonAndYear(Season Season);
    }
}
