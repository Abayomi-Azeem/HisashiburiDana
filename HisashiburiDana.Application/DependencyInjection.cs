using HisashiburiDana.Application.Abstractions.Application;
using HisashiburiDana.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {


            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAniListService, AniListService>();
            services.AddScoped<IUserAnimeService, UserAnimeService>();
            return services;
        }
    }
}
