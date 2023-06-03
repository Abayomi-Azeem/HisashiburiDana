using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using HisashiburiDana.Application.Abstractions.Infrastucture.Authentication;
using HisashiburiDana.Application.Abstractions.Infrastucture.IHelpers;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence.IRepository;
using HisashiburiDana.Application.Abstractions.Infrastucture.ThirdPartyDependencies;
using HisashiburiDana.Contract.Common;
using HisashiburiDana.Infrastructure.Authentication;
using HisashiburiDana.Infrastructure.Helpers;
using HisashiburiDana.Infrastructure.Persistence;
using HisashiburiDana.Infrastructure.Persistence.Repository;
using HisashiburiDana.Infrastructure.ThirdPartyDependecies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.AddTransient<IAnimeListManager, AnimeListManager>();
            services.AddSingleton<IRequestSender, RequestSender>();

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

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("JstSettings:Issuer").Value,
                    ValidAudience = configuration.GetSection("JstSettings:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(
                                            Encoding.UTF8.GetBytes(configuration.GetSection("JstSettings:SecretKey").Value)
                    )
                });


            return services;
        }
    }
}
