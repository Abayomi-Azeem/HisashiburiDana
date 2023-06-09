using Amazon.DynamoDBv2;
using HisashiburiDana.Application.Abstractions.Infrastucture.IPersistence.IRepository;
using HisashiburiDana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure.Persistence.Repository
{
    public class AlreadyWatchedAnimeRepository: GenericRepository<WatchedAnimes>, IAlreadyWatchedAnimeRepository
    {
        public AlreadyWatchedAnimeRepository(IAmazonDynamoDB client): base(client)
        {

        }
    }
}
