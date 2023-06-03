﻿using GraphQL;
using HisashiburiDana.Application.Abstractions.Infrastucture.IHelpers;
using HisashiburiDana.Application.Abstractions.Infrastucture.ThirdPartyDependencies;
using HisashiburiDana.Contract.AnimeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure.ThirdPartyDependecies
{
    public class AnimeListManager: IAnimeListManager
    {
        private readonly IRequestSender _sender;

        public AnimeListManager(IRequestSender sender)
        {
            _sender = sender;
        }

        public async Task<AllGenres?> GetAllGenres()
        {
            var request = new GraphQLRequest
            {
                Query = @"{
                              GenreCollection
                          }",
            };

            var response = await _sender.SendGraphQLMessage<AllGenres>(request);

            if (response.Errors != null)
            {
                return response.Data;
            }
            return null;

        }
    
        public async Task<AnimeList> GetAnimes(string pageNumber)
        {
            string baseQuery = @"{
    Page(page:[pageNumber], perPage:20){
        pageInfo{
            total
            currentPage
            lastPage
            hasNextPage
            perPage
        }
    
    media{
        id
        title{
            english
            romaji
            }
        description
        startDate{
            day
            month
            year
        }
        endDate{
            day
            month
            year
        }
        status
        siteUrl
        coverImage{
            medium
        }
        episodes
        genres
        rankings{
            rank
            allTime
        } 
        siteUrl           
    }      
  }
}";
            var request = new GraphQLRequest
            {
                Query= baseQuery.Replace("[pageNumber]", pageNumber)
                            };

            var response = await _sender.SendGraphQLMessage<AnimeList>(request);

            if (response.Errors == null)
            {
                return response.Data;
            }
            return null;
        }
    }
}
