using Amazon.DynamoDBv2.DataModel;
using HisashiburiDana.Contract.Authentication;
using HisashiburiDana.Domain.Common;
using HisashiburiDana.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Domain.Entities
{
    [DynamoDBTable("Users")]
    public class User 
    {
        private User(Guid id, string firstname, string lastname, string password, string email, string passwordSalt) 
        {
            Id = id.ToString();
            FirstName = firstname;
            LastName = lastname;
            Password = password;
            Email = email;
            PasswordSalt = passwordSalt;
            DateCreated = DateTime.UtcNow.AddHours(1);
        }

        public User() { }

        [DynamoDBHashKey]
        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Password { get; private set; }

        public string PasswordSalt { get; private set; }

        public string Email { get; private set; }

        public DateTime DateCreated { get; private set; }


        public static User Create(string firstname, string lastname, string email, string passwordhash, string passwordSalt)
        {
            return new(Guid.NewGuid(), firstname, lastname, passwordhash, email, passwordSalt);
        }
    }
}
