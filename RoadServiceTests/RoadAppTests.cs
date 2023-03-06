using Application.Contracts.Roads;
using Infrastructure.Services.Roads;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using TFLTestApp;
using TFLTestApp.App;
using System.Text.Json;
using Domain.Models;
using Shouldly;
using TFLTestApp.ExitCodes;
using TFLTestApp.Services;
using MediatR;
using Application.Features.Roads.Queries.GetRoadStatus;
using Application.Dtos;
using Application.Responses;
using System.Collections.Generic;

namespace RoadServiceTests
{
    public class RoadAppTests
    {
       
        IRoadApp _roadApp;
        IAsyncRoadService _asyncRoadService;
        Mock<IRoadDataService> _roadDataService;

        Mock<IHttpClientFactory> _httpClientFactory;
        HttpClient _httpClient;
        Mock<HttpMessageHandler> _handler;

        Mock<ILogger<IRoadApp>> _logger;
        IOptions<AppSettings> _appSettings;

        public const string ExistingCorridor = "A2";
        public const string NonExistentCorridor = "A90000000";
        public const string TFLBaseRoadUrl = "https://api.tfl.gov.uk/Road/";
        public const string AppKey = "appKeyTest";
        public const string AppId = "appIdTest";
        public RoadAppTests()
        {
            _appSettings = Options.Create(new AppSettings() { AppKey = AppKey, 
                AppId = AppId, 
                TFLBaseRoadUrl = TFLBaseRoadUrl
            });

            _logger = new Mock<ILogger<IRoadApp>>();

        }

        [Fact]
        public async Task RoadAppRequestRoadExistsTest()
        {
            //arrange
            _handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handler.Object);
            _httpClient.BaseAddress = new Uri(_appSettings.Value.TFLBaseRoadUrl);

            //_httpClient.Setup(x => x.SendAsync()).Returns(Task.CompletedTask);
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _asyncRoadService = new AsyncRoadService(_httpClientFactory.Object);


            

            var roadDto = new RoadCorridorDto()
            {
                Id = ExistingCorridor,
                StatusSeverity = "Good",
                StatusSeverityDescription = "No Exceptional Delays"
            };

            ICollection<RoadCorridorDto> roadDtoCollection = new List<RoadCorridorDto>() { roadDto };
            var response = new Response<ICollection<RoadCorridorDto>>(roadDtoCollection);
            _roadDataService = new Mock<IRoadDataService>();
            _roadDataService.Setup(x => x.GetRoadCorridorAsync(It.IsAny<GetRoadStatusParameter>()))
                .ReturnsAsync(response);

            _roadApp = new RoadApp(_appSettings, _logger.Object, _asyncRoadService, _roadDataService.Object);
            var args = new string[] { ExistingCorridor };

            //act
            var result = await _roadApp.Run(args);

            //assert
            result.ShouldBe((int)ExitCode.Success);

        }
        [Fact]
        public async Task RoadAppRequestRoadDoesNotExistTest()
        {
            //arrange
            _handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handler.Object);
            _httpClient.BaseAddress = new Uri(_appSettings.Value.TFLBaseRoadUrl);
           
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _asyncRoadService = new AsyncRoadService(_httpClientFactory.Object);

            ICollection<RoadCorridorDto> roadDtoCollection = new List<RoadCorridorDto>() { };
            var response = new Response<ICollection<RoadCorridorDto>>(roadDtoCollection);
            _roadDataService = new Mock<IRoadDataService>();
            _roadDataService.Setup(x => x.GetRoadCorridorAsync(It.IsAny<GetRoadStatusParameter>()))
                .ReturnsAsync(response);

            _roadApp = new RoadApp(_appSettings, _logger.Object, _asyncRoadService, _roadDataService.Object);
            var args = new string[] { NonExistentCorridor };

            //act
            var result = await _roadApp.Run(args);

            //assert
            result.ShouldBe((int)ExitCode.Failed);

        
        }

        [Fact]
        public async Task RoadAppExceptionThrownTest()
        {
            //arrange
            
            _asyncRoadService = null;
            var mediator = new Mock<IMediator>();
            _roadDataService = new Mock<IRoadDataService>();

            _roadApp = new RoadApp(_appSettings, _logger.Object, _asyncRoadService, _roadDataService.Object);
            var args = new string[] { ExistingCorridor };

            //act
            Should.Throw<Exception>(_roadApp.Run(args));


        }
    

        
    }
}