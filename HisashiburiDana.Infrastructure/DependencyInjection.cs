using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using HisashiburiDana.Application.Abstractions.Infrastucture.Authentication;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence.IRepository;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Infrastructure.Authentication;
using HisashiburiDana.Infrastructure.Persistence;
using HisashiburiDana.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            var accessKey = configuration.GetSection("AwsSettings:AccessKey").Value;
            var secretkey = configuration.GetSection("AwsSettings:SecretKey").Value;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretkey);
            var awsConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2 // Set your desired region
            };

            services.AddAWSService<IAmazonDynamoDB>(new AWSOptions
            {
                Credentials = awsCredentials,
                Region = awsConfig.RegionEndpoint
            });

            //services.AddDbContext<AnimeDbContext>(
            //    options => options.UseSqlServer(configuration.GetConnectionString("AnimeDbConnString"), sqlOptions => sqlOptions.EnableRetryOnFailure(50))
            //    );
            return services;
        }
    }
}
