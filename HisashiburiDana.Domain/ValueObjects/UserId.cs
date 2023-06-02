using Amazon.DynamoDBv2.DataModel;
using HisashiburiDana.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Domain.ValueObjects
{
    public class UserId : ValueObject
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("Id")]
        public string Value { get; }

        public UserId()
        {

        }
        private UserId(string value)
        {
            Value = value;
        }

        public static UserId CreateUserId()
        {
            return new(Guid.NewGuid().ToString());
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static UserId Create(Guid value)
        {
            return new UserId(value.ToString());
        }
    }
}
