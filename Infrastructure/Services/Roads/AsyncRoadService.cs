using Application.Contracts.Roads;
using Domain.Models;
using Newtonsoft.Json;

namespace Infrastructure.Services.Roads
{
    public class AsyncRoadService : IAsyncRoadService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public AsyncRoadService(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ICollection<RoadCorridor>> GetRoadCorridorAsync(string roadId,
            string appid, 
            string appkey)
        {
            var httpClient = _httpClientFactory.CreateClient("RoadServiceHttpClient");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            //var httpResponseMessage = await httpClient.GetAsync($"{roadId}?app_id={appid}&app_key={appkey}");
            var httpResponseMessage = await httpClient.GetAsync($"{roadId}?app_key={appkey}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var result = JsonConvert.DeserializeAnonymousType(new StreamReader(contentStream).ReadToEnd(), 
                    new RoadCorridor[] { new RoadCorridor() }); 

                return result != null && result.Any() ? new List<RoadCorridor>() { result[0] } : new List<RoadCorridor>() {};
            }

            return new List<RoadCorridor>() { };
        }
    }
    
}
