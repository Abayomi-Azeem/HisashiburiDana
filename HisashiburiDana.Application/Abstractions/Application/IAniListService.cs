﻿using HisashiburiDana.Contract.AnimeManager;
using HisashiburiDana.Contract.Common;
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
    }
}
