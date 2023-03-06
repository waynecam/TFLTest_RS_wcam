using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Responses;
using MediatR;

namespace Application.Features.Roads.Queries.GetRoadStatus
{
    public class GetRoadStatusQuery : IRequest<Response<ICollection<RoadCorridorDto>>>
    {

        public GetRoadStatusParameter GetRoadStatusParameter { get; set; }
        public GetRoadStatusQuery(GetRoadStatusParameter getRoadStatusParameter)
        {
            GetRoadStatusParameter = getRoadStatusParameter;
        }
    }
}
