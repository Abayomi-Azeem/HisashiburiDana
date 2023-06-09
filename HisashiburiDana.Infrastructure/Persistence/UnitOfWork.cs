﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using HisashiburiDana.Application.Abstractions.Infrastucture.IPersistence.IRepository;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DynamoDBContext _context;

        public UnitOfWork(IAmazonDynamoDB client, IUserRepository userRepo, IAnimeRankRepository animeRepo, IToWatchAnimeRepository toWatchAnime, ICurrentlyWatchingAnimeRepository watchingAnime, IAlreadyWatchedAnimeRepository watchedAnime)
        {
            _context = new DynamoDBContext(client);
            UserRepo = userRepo;
            ToWatchAnimeRepo = toWatchAnime;
            AnimeRankingsRepo = animeRepo;
            WatchedAnimeRepo = watchedAnime;
            WatchingAnimeRepo = watchingAnime;

        }





        public IUserRepository UserRepo { get; set; }

        public IToWatchAnimeRepository ToWatchAnimeRepo { get; set; }

        public IAnimeRankRepository AnimeRankingsRepo { get; set; }

        public IAlreadyWatchedAnimeRepository WatchedAnimeRepo { get; set; }

        public ICurrentlyWatchingAnimeRepository WatchingAnimeRepo { get; set; }
        
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        public async Task SaveChanges<T>(T value )
        {
             await _context.SaveAsync(value);
        }
    }
}
