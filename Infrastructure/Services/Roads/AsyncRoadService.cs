using Application.Contracts.Roads;
using Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

            var httpResponseMessage = await httpClient.GetAsync($"{roadId}?app_id={appid}&app_key={appkey}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync
                    <RoadCorridor>(contentStream);

                /*Returning nullResponse if response is successful but nothing to actually deserialize - suggesting issue downstream with the api
                 *probably requires business analyst input to decide how to actually handle this type response under aforementioned circumstances*/
                return result != null ? new List<RoadCorridor>() { result } : new List<RoadCorridor>() {};
            }

            //return new List<IRoadCorridor>() { new RoadCorridor() { Id= roadId, StatusSeverity = "Good", StatusSeverityDescription = "No Exceptional Delays" } };
            return new List<RoadCorridor>() { };
        }
    }
    
}
