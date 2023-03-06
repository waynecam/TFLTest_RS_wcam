using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Net.Http;
using System.Collections.Specialized;
using Application.Contracts.Roads;
using Infrastructure.Services.Roads;

namespace Infrastructure
{
    public static class InfrastructureRegistrationService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //get the config file
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            var UseInMemoryDatasource = !string.IsNullOrEmpty(config.GetValue<string>("UseInMemoryDataSource")) 
                ? Convert.ToBoolean(config.GetValue<string>("UseInMemoryDataSource")) :
                true;

            if(UseInMemoryDatasource)
            {

                services.AddTransient<IAsyncRoadService, AsyncInMemoryRoadService>();
                
            }
            else
            {
                services.AddTransient<IAsyncRoadService, AsyncRoadService>();
            }
            
            

            return services;
        }
    }
}
