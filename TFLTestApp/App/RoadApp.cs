using Application.Contracts.Roads;
using Application.Features.Roads.Queries.GetRoadStatus;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFLTestApp.App;
using TFLTestApp.ExitCodes;
using TFLTestApp.Services;

namespace TFLTestApp.App
{
    public class RoadApp : IRoadApp
    {
        private readonly IAsyncRoadService _roadService;

        private readonly AppSettings _appSettings;
        private readonly ILogger<IRoadApp> _logger;

        private IRoadDataService _roadDataService;

        public RoadApp(IOptions<AppSettings> appSettings,
            ILogger<IRoadApp> logger,
            IAsyncRoadService roadService,
            IRoadDataService roadDataService
            )
        {
            _roadService = roadService;
            _appSettings = appSettings.Value;
            _logger = logger;
            _roadDataService = roadDataService;
        }

        public async Task<int> Run(string[] args)
        {
            string roadCorridor;

            if (args.Length == 0)
            {
                Console.Write("Enter Road Corridor: ");
                roadCorridor = Console.ReadLine();
            }
            else
            {
                roadCorridor = args[0];
            }

            var result = await _roadDataService.GetRoadCorridorAsync(new GetRoadStatusParameter(roadCorridor, 
                _appSettings.AppId, 
                _appSettings.AppKey));

            var data = result.Data;

            if(data.Any())
            {
                Console.WriteLine($"{data.First().RoadStatusMessage}");
                Console.WriteLine("\n");

                return (int)ExitCode.Success;
            }
            else
            {
                Console.WriteLine($"{roadCorridor} is not a valid road");
                return (int)ExitCode.Failed;
            }

            

            


        }


    }
}
