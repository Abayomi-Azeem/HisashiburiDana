﻿using Amazon.DynamoDBv2.DataModel;
using HisashiburiDana.Contract.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Domain.Entities
{
    [DynamoDBTable("WatchingAnimes")]
    public class WatchingAnimes
    {
        public WatchingAnimes(AddAnimeFromAniList request)
        {
            Id = Guid.NewGuid().ToString();
            UserId = request.UserId;
            Title = String.Join(",", request.Media.Title.English, request.Media.Title.Romaji);
            Description = request.Media.Description;
            StartDate = new DateTime(request.Media.StartDate.Year ?? 0, request.Media.StartDate.Month ?? 0, request.Media.StartDate.Day ?? 0);
            EndDate = new DateTime(request.Media.EndDate.Year ?? 0, request.Media.EndDate.Month ?? 0, request.Media.EndDate.Day ?? 0);
            Status = request.Media.Status;
            SiteUrl = request.Media.SiteUrl;
            CoverUrl = request.Media.CoverImage.ExtraLarge;
            Episodes = request.Media.Episodes;
            Genres = String.Join(",", request.Media.Genres);
            DateAdded = DateTime.UtcNow.AddHours(1);
            DateStartedWatching = DateTime.UtcNow.AddHours(1);
        }

        public WatchingAnimes(ToWatchAnimes toWatchAnimes)
        {
            Id = Guid.NewGuid().ToString();
            UserId = toWatchAnimes.UserId;
            Title = toWatchAnimes.Title;
            Description = toWatchAnimes.Description;
            StartDate = toWatchAnimes.StartDate;
            EndDate = toWatchAnimes.EndDate;
            Status = toWatchAnimes.Status;
            SiteUrl = toWatchAnimes.SiteUrl;
            CoverUrl = toWatchAnimes.CoverUrl;
            Episodes = toWatchAnimes.Episodes;
            Genres = toWatchAnimes.Genres;
            DateAdded = DateTime.UtcNow.AddHours(1);
            DateStartedWatching = toWatchAnimes.DateAdded;
        }

        public WatchingAnimes()
        {

        }

        

        [DynamoDBHashKey]
        public string Id { get; private set; }

        public string UserId { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public DateTime DateAdded { get; private set; }
    
        public DateTime? DateStartedWatching { get; private set; }

        public string Status { get; private set; }

        public string SiteUrl { get; private set; }

        public string CoverUrl { get; private set; }

        public int? Episodes { get; private set; }

        public string Genres { get; private set; }

        public string RankingId { get; set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }


        public static WatchingAnimes Create(AddAnimeFromAniList request)
        {
            return new(request);
        }

        public static WatchingAnimes AddRankingId(WatchingAnimes anime, List<string> rankIds)
        {
            anime.RankingId = string.Join(",", rankIds);
            return anime;
        }

        public static WatchingAnimes Create(ToWatchAnimes toWatchAnimes)
        {
            return new(toWatchAnimes);
        }
    }
}
