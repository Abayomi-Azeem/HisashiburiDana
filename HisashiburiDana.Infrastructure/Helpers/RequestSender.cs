using GraphQL;
using GraphQL.Client;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using HisashiburiDana.Application.Abstractions.Infrastucture.IHelpers;
using HisashiburiDana.Contract.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure.Helpers
{
    public class RequestSender : IRequestSender
    {
        private readonly GraphQLHttpClient _graphqlClient;

        public RequestSender(IConfiguration configuration)
        {
            if (configuration.GetSection("AnimeListSettings:Uri").Value == null)
            {
                throw new Exception("Animelist Uri Cannot be Null");
            }
            _graphqlClient = new GraphQLHttpClient(configuration.GetSection("AnimeListSettings:Uri").Value, new NewtonsoftJsonSerializer());
        }

       
        public async Task<GraphQLResponse<T>> SendGraphQLMessage<T>(GraphQLRequest query)
        {
            var response = await _graphqlClient.SendQueryAsync<T>(query);

            return response;
            
        }
    }
}
