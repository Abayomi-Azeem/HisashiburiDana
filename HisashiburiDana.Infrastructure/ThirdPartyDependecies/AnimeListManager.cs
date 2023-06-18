using GraphQL;
using HisashiburiDana.Application.Abstractions.Infrastucture.IHelpers;
using HisashiburiDana.Application.Abstractions.Infrastucture.ThirdPartyDependencies;
using HisashiburiDana.Contract.AnimeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HisashiburiDana.Contract.Enumerations.StatusEnum;

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

            if (response.Errors == null)
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
            extraLarge
        }
        episodes
        genres
        rankings{
            rank
            allTime
            context
            type
            format
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

        public async Task<AnimeList> GetTrendingAnimes()
        {
            string query = @"{
    Page(page:1, perPage:10){
        pageInfo{
            total
            currentPage
            lastPage
            hasNextPage
            perPage
        }
    
    media(sort: TRENDING_DESC){
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
            extraLarge
        }
        episodes
        genres
        rankings{
            rank
            allTime
            context
            type
            format
        } 
        siteUrl           
    }      
  }
}";
            var request = new GraphQLRequest
            {
                Query = query
            };

            var response = await _sender.SendGraphQLMessage<AnimeList>(request);

            if (response.Errors == null)
            {
                return response.Data;
            }
            return null;
        }

        public async Task<AnimeList> SearchAnimes(string animeName)
        {
            string query = $@"{{
        Page(page:1, perPage:20){{
            pageInfo{{
                total
                currentPage
                lastPage
                hasNextPage
                perPage
            }}
    
            media(search: ""{animeName}"") {{
                id
                title {{
                    english
                    romaji
                }}
                description
                startDate {{
                    day
                    month
                    year
                }}
                endDate {{
                    day
                    month
                    year
                }}
                status
                siteUrl
                coverImage {{
                    extraLarge
                }}
                episodes
                genres
                rankings {{
                    rank
                    allTime
                    context
                    type
                    format
                }} 
                siteUrl           
            }}      
        }}
    }}";


            var request = new GraphQLRequest
            {
                Query = query
            };

            var response = await _sender.SendGraphQLMessage<AnimeList>(request);

            if (response.Errors == null)
            {
                return response.Data;
            }
            return null;
        }

        public async Task<AnimeList> FilterAnimes(FilterRequest payload)
        {
            string query = $@"{{
        Page(page:1, perPage:20){{
            pageInfo{{
                total
                currentPage
                lastPage
                hasNextPage
                perPage
            }}

            media(";

            if (!string.IsNullOrEmpty(payload.Genre))
            {
                query += $" genre: \"{payload.Genre}\"";
            }

            if (payload.EpisodesGreaterThan > 0 || payload.EpisodesGreaterThan != null)
            {
                query += $"{(string.IsNullOrEmpty(payload.Genre) ? "" : ",")} episodes_greater: {payload.EpisodesGreaterThan}";
            }

            if (payload.Status != null)
            {
                query += $"{(string.IsNullOrEmpty(payload.Genre) && payload.EpisodesGreaterThan == null || payload.EpisodesGreaterThan == null ? "" : ",")} status: {payload.Status}";
            }

            if (payload.isAdult != null)
            {
                query += $@"{(string.IsNullOrEmpty(payload.Genre) && payload.EpisodesGreaterThan == null || payload.EpisodesGreaterThan == null && payload.Status == null ? "" : ",")} isAdult: {Convert.ToString(payload.isAdult).ToLower()}";
            }

            query += @") {
                id
                title {
                    english
                    romaji
                }
                description
                startDate {
                    day
                    month
                    year
                }
                endDate {
                    day
                    month
                    year
                }
                status
                siteUrl
                coverImage {
                    extraLarge
                }
                episodes
                genres
                rankings {
                    rank
                    allTime
                    context
                    type
                    format
                } 
                siteUrl           
            }      
        }
    }";

            var request = new GraphQLRequest
            {
                Query = query
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
