using HisashiburiDana.Application.Abstractions.Infrastucture.IPersistence.IRepository;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepo {get; }

        IUserAnimeRepository UserAnimeRepo { get; }

        IAnimeRankRepository AnimeRankingsRepo { get;  }

        Task SaveChanges<T>(T value);
    }
}
