﻿using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Contract.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Application
{
    public interface IAniListService
    {
        Task<GeneralResponseWrapper<AnimeList>> GetAnimes(int pageNumber);

        Task<GeneralResponseWrapper<AllGenres>> GetAllGenres();
        Task<GeneralResponseWrapper<AnimeList>> GetTrendingAnime();
        Task<GeneralResponseWrapper<AnimeList>> SearchInAnimeList(string animeName);
        Task<GeneralResponseWrapper<AnimeList>> GetSortedAnimes(Sorter sortBy, int pageNumber);
        Task<GeneralResponseWrapper<AnimeList>> FilterAnime(FilterRequest payload);
    }
}
