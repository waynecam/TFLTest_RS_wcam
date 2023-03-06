using Application.Contracts.Roads;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Roads
{
    public class AsyncInMemoryRoadService : IAsyncRoadService
    {

        private readonly List<RoadCorridor> InMemoryRoadsThatExisit;

        private readonly IHttpClientFactory _httpClientFactory;
        public AsyncInMemoryRoadService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            InMemoryRoadsThatExisit = new List<RoadCorridor>()
            {
                new RoadCorridor {Id = "A2", StatusSeverity = "Good", StatusSeverityDescription = "No Exceptional Delays"},
                new RoadCorridor {Id = "A3", StatusSeverity = "Bad", StatusSeverityDescription = "Exceptional Delays"},
                new RoadCorridor {Id = "A5", StatusSeverity = "Fair", StatusSeverityDescription = "No Exceptional Delays"}
            };
          
        }


        
        public async Task<ICollection<RoadCorridor>> GetRoadCorridorAsync(string roadId, string appid, string appkey)
        {
            if(InMemoryRoadsThatExisit.Any(x => x.Id == roadId))
            {
                return (ICollection<RoadCorridor>)await Task.FromResult(InMemoryRoadsThatExisit.Where(x => x.Id == roadId).Select(x => x).ToList());
            }
            else
            {
                return await Task.FromResult(new List<RoadCorridor>() { });
            }
        }
    }
}
