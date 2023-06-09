using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence.IRepository;
using HisashiburiDana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.IPersistence.IRepository
{
    public interface ICurrentlyWatchingAnimeRepository: IGenericRepository<WatchingAnimes>
    {

    }
}
