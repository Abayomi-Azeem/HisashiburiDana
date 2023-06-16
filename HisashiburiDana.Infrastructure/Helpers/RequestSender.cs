using GraphQL;
using GraphQL.Client;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using HisashiburiDana.Application.Abstractions.Infrastucture.IHelpers;
using HisashiburiDana.Contract.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly ILogger<RequestSender> _logger;

        public RequestSender(IConfiguration configuration, ILogger<RequestSender> logger)
        {
            if (configuration.GetSection("AnimeListSettings:Uri").Value == null)
            {
                throw new Exception("Animelist Uri Cannot be Null");
            }
            _graphqlClient = new GraphQLHttpClient(configuration.GetSection("AnimeListSettings:Uri").Value, new NewtonsoftJsonSerializer());
            _logger = logger;
        }


        public async Task<GraphQLResponse<T>> SendGraphQLMessage<T>(GraphQLRequest query)
        {
            try
            {
                var response = await _graphqlClient.SendQueryAsync<T>(query);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SendingGraphQLMessage Exception Caught: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
            
        }
    }
}
