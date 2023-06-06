using Amazon.DynamoDBv2.DataModel;
using HisashiburiDana.Contract.AnimeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Domain.Entities
{
    [DynamoDBTable("Rankings")]
    public class Rankings
    {
        public Rankings(Ranking request, string userId)
        {
            Id = Guid.NewGuid().ToString();
            AllTime = request.AllTime;
            Context = request.Context;
            Type = request.Type;
            Format = request.Format;
            UserId = userId;

        }

        public Rankings() {  }


        [DynamoDBHashKey]
        public string Id { get; private set; }

        public bool? AllTime { get; private set; }

        public string? Context { get; private set; }

        public string? Type { get; private set; }

        public string? Format { get; private set; }

        public string UserId { get; private set; }

        
        public static Rankings Create(Ranking request, string userId)
        {
            return  new(request, userId);
        }
    }
}
