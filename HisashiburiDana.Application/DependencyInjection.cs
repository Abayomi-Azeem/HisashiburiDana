using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Services;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using System.Reflection;

namespace HisashiburiDana.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {


            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAniListService, AniListService>();
            services.AddScoped<IUserAnimeService, UserAnimeService>();

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
