using Application.Contracts.Roads;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TFLTestApp.App;
using TFLTestApp;
using Domain.Models;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Infrastructure.Services.Roads;
using Shouldly;

namespace RoadServiceTests
{
    public class RoadServiceTests
    {

        IAsyncRoadService _asyncRoadService;

        Mock<IHttpClientFactory> _httpClientFactory;
        HttpClient _httpClient;
        Mock<HttpMessageHandler> _handler;
        IOptions<AppSettings> _appSettings;

        private const string ExistingCorridor = "A2";
        private const string ExistingCorridorDisplayName = "A2";
        private const string NonExistentCorridor = "A90000000";
        private const string NonExistentCorridorDisplayName = "A90000000";
        private const string TFLBaseRoadUrl = "https://api.tfl.gov.uk/Road/";
        private const string AppKey = "appKeyTest";
        private const string AppId = "appIdTest";

        public RoadServiceTests()
        {
            _appSettings = Options.Create(new AppSettings()
            {
                AppKey = AppKey,
                AppId = AppId,
                TFLBaseRoadUrl = TFLBaseRoadUrl
            });
        }


        [Fact]
        public async Task GetExistingCorridorTest()
        {
            //arrange
            _handler = SetUpCorriderExistsMockHttpHandler();
            _httpClient = new HttpClient(_handler.Object);
            _httpClient.BaseAddress = new Uri(_appSettings.Value.TFLBaseRoadUrl);

            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _asyncRoadService = new AsyncRoadService(_httpClientFactory.Object);

            //act
            var result = await _asyncRoadService.GetRoadCorridorAsync(ExistingCorridor, AppId, AppKey);

            //assert
            result.Count().ShouldBe(1);
            result.First().ShouldBeOfType(typeof(RoadCorridor));
            result.First().Id.ShouldBe(ExistingCorridor);
            result.First().DisplayName.ShouldBe(ExistingCorridor);
            result.First().StatusSeverity.ShouldBe("Good");
            result.First().StatusSeverityDescription.ShouldBe("No Exceptional Delays");

            var message = $"The status of the {ExistingCorridorDisplayName} is as follows {Environment.NewLine}" +
                $"{Indent(3)}The road Status is Good {Environment.NewLine}" +
                $"{Indent(3)}The road Status Description is No Exceptional Delays";

            result.First().RoadStatusMessage.ShouldContain(message);

        }


        [Fact]

        public async Task GetNonExistentCorridorTest()
        {
            //arrange
            _handler = SetUpCorriderDoesNotExistMockHttpHandler();
            _httpClient = new HttpClient(_handler.Object);
            _httpClient.BaseAddress = new Uri(_appSettings.Value.TFLBaseRoadUrl);

            
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);
            _asyncRoadService = new AsyncRoadService(_httpClientFactory.Object);

            //act
            var result = await _asyncRoadService.GetRoadCorridorAsync(NonExistentCorridor, AppId, AppKey);

            //assert
            result.Count().ShouldBe(0);
        }
        private static Mock<HttpMessageHandler> SetUpCorriderExistsMockHttpHandler()
        {
            var responsePoco = new RoadCorridor() { Id = ExistingCorridor, DisplayName = ExistingCorridorDisplayName, StatusSeverity = "Good", StatusSeverityDescription = "No Exceptional Delays" };
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
            var responsePoco = new RoadCorridor() { Id = NonExistentCorridor, DisplayName = NonExistentCorridorDisplayName };
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
