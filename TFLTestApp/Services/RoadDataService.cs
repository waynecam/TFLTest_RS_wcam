using Application.Dtos;
using Application.Features.Roads.Queries.GetRoadStatus;
using Application.Responses;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFLTestApp.Services
{
    public class RoadDataService : IRoadDataService
    {
        private readonly IMediator _mediatr;
        public RoadDataService(IMediator mediatr)
        {
            _mediatr = mediatr;
        }
        public async Task<Response<ICollection<RoadCorridorDto>>> GetRoadCorridorAsync(GetRoadStatusParameter getRoadStatusParameter)
        {
            var response = await _mediatr.Send(new GetRoadStatusQuery(getRoadStatusParameter));

            return response;
        }
    }
}
