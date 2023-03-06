using Application;
using Application.Contracts.Roads;
using Infrastructure;
using Infrastructure.Services.Roads;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging;
using TFLTestApp;
using TFLTestApp.App;
using TFLTestApp.ExitCodes;
using TFLTestApp.Services;
// See https://aka.ms/new-console-template for more information


static void ConfigureServices(IServiceCollection services)
{
    // configure logging
    services.AddLogging(builder =>
    {
        builder.AddConsole();
        //builder.AddDebug();
        builder.AddFilter(x => x == LogLevel.Error);
    });

    // build config
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        //.AddEnvironmentVariables()
        .Build();

    services.AddSingleton<IConfiguration>(configuration);


    services.Configure<AppSettings>(configuration.GetSection("App"));

    // add services:
    services.AddSingleton<IRoadApp, RoadApp>();
    //services.AddTransient<IAsyncRoadService, AsyncInMemoryRoadService>();
    services.AddInfrastructureServices();
    services.AddApplicationServices();

    services.AddTransient<IRoadDataService, RoadDataService>();

    services.AddHttpClient("RoadServiceHttpClient", httpClient =>
    {
        httpClient.BaseAddress = new Uri(configuration.GetValue<string>("App:TFLBaseRoadUrl"));
    });

    // add app
    services.AddTransient<RoadApp>();
}

// create service collection
var services = new ServiceCollection();
ConfigureServices(services);

// create service provider
using var serviceProvider = services.BuildServiceProvider();

// entry to run app
var _result = (int)ExitCode.Success;
ILogger<Program> _logger = serviceProvider.GetService<ILogger<Program>>();

try
{
    _result = await serviceProvider.GetService<IRoadApp>().Run(args);
    Environment.ExitCode = _result;
}
catch (Exception ex)
{
    var logger = serviceProvider.GetService<ILogger>();
    _logger.LogError($"Error Running The App: Please Contact Admin");
    //Ordinarily this wouldnt be echoed
    _logger.LogError($"Error Message {ex.StackTrace}");
    _result = (int)ExitCode.Failed;
    Environment.ExitCode = 1;
}

return _result;

