using Amazon.DynamoDBv2.DataModel;
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
        [DynamoDBHashKey]
        public string Id { get; private set; }

        public string UserId { get; private set; }

        public string Name { get; private set; }

        public DateTime DateAdded { get; private set; }
    
        public DateTime DateStartedWatgching { get; private set; }
    }
}
