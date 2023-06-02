using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure.Persistence.Models
{
    [DynamoDBTable("Users")]
    public class UserModel
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string FirstName { get;  set; }

        public string LastName { get;  set; }

        public string Password { get;  set; }

        public string PasswordSalt { get;  set; }

        public string Email { get;  set; }

        public DateTime DateCreated { get; set; }
    }
}
