using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.IHelpers
{
    public interface IRequestSender
    {
        Task<GraphQLResponse<T>> SendGraphQLMessage<T>(GraphQLRequest query);
    }
}
