﻿using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.AnimeList
{
    public interface IAnimeList
    {
        IEnumerable<Datum> AnimeLists { get; set; }
        Task GetAnimeListBySeasonAndYear(Season season);
    }
}