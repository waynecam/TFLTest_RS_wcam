using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Application.MappingProfiles;
using System.Reflection;

namespace Application
{
    public static class ApplicationRegistrationService
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
