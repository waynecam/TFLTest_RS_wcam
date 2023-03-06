using Application.Contracts.Roads;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFLTestApp.App;
using TFLTestApp;
using Xunit.Sdk;
using Application.Features.Roads.Queries.GetRoadStatus;
using AutoMapper;
using Application.MappingProfiles;
using System.Runtime.InteropServices;
using Shouldly;
using Application.Dtos;
using Infrastructure.Services.Roads;
using Domain.Models;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Application.Responses;

namespace RoadServiceTests
{
    public class RoadTestCQRSTests
    {

        Mock<IHttpClientFactory> _httpClientFactory;
        HttpClient _httpClient;
        Mock<HttpMessageHandler> _handler;
        IOptions<AppSettings> _appSettings;
        IAsyncRoadService _asyncRoadService;
        IMapper _mapper;

        private const string ExistingCorridor = "A2";
        private const string NonExistentCorridor = "A90000000";
        private const string TFLBaseRoadUrl = "https://api.tfl.gov.uk/Road/";
        private const string AppKey = "appKeyTest";
        private const string AppId = "appIdTest";


        public RoadTestCQRSTests()
        {
            _appSettings = Options.Create(new AppSettings()
            {
                AppKey = AppKey,
                AppId = AppId,
                TFLBaseRoadUrl = TFLBaseRoadUrl
            });

           

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper  = mapperConfig.CreateMapper();

        }

        [Fact]
        public async Task GetExistingCorridorHandlerTest()
        {
            _handler = SetUpCorriderExistsMockHttpHandler();
            _httpClient = new HttpClient(_handler.Object);
            _httpClient.BaseAddress = new Uri(_appSettings.Value.TFLBaseRoadUrl);

            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _asyncRoadService = new AsyncRoadService(_httpClientFactory.Object);

            var handler = new GetRoadStatusQueryHandler(_asyncRoadService, _mapper);

            var result = await handler.Handle(new GetRoadStatusQuery(new GetRoadStatusParameter(ExistingCorridor, AppId, AppKey)), CancellationToken.None);

            result.ShouldBeOfType<Response<ICollection<RoadCorridorDto>>>();

            var message = $"The status of the {ExistingCorridor} is as follows {Environment.NewLine}" +
                $"{Indent(3)}The road Status is Good {Environment.NewLine}" +
                $"{Indent(3)}The road Status Description is No Exceptional Delays";

            result.Data.First().RoadStatusMessage.ShouldContain(message);


        }

        [Fact]
        public async Task GetNonExistentCorridorHandlerTest()
        {
            _handler = SetUpCorriderDoesNotExistMockHttpHandler();
            _httpClient = new HttpClient(_handler.Object);
            _httpClient.BaseAddress = new Uri(_appSettings.Value.TFLBaseRoadUrl);

            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _asyncRoadService = new AsyncRoadService(_httpClientFactory.Object);

            var handler = new GetRoadStatusQueryHandler(_asyncRoadService, _mapper);

            var result = await handler.Handle(new GetRoadStatusQuery(new GetRoadStatusParameter(NonExistentCorridor, AppId, AppKey)), CancellationToken.None);

            result.ShouldBeOfType<Response<ICollection<RoadCorridorDto>>>();

            result.Data.Count().ShouldBe(0);

        }

        private static Mock<HttpMessageHandler> SetUpCorriderExistsMockHttpHandler()
        {
            var responsePoco = new RoadCorridor() { Id = ExistingCorridor, StatusSeverity = "Good", StatusSeverityDescription = "No Exceptional Delays" };
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(JsonSerializer.Serialize(responsePoco)),
               })
               .Verifiable();

            return handlerMock;
        }

        private static Mock<HttpMessageHandler> SetUpCorriderDoesNotExistMockHttpHandler()
        {
            var responsePoco = new RoadCorridor() { Id = NonExistentCorridor };
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.NotFound,
                   Content = new StringContent(JsonSerializer.Serialize(responsePoco)),
               })
               .Verifiable();

            return handlerMock;
        }

        private static string Indent(int count)
        {
            return "".PadLeft(count);
        }

    }
}
