using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Roads;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.Features.Roads.Queries.GetRoadStatus
{
    public class GetRoadStatusQueryHandler : IRequestHandler<GetRoadStatusQuery, Response<ICollection<RoadCorridorDto>>>
    {
        IAsyncRoadService _asyncRoadService;
        IMapper _mapper;
        public GetRoadStatusQueryHandler(IAsyncRoadService asyncRoadService,
            IMapper mapper
            )
        {
            _asyncRoadService = asyncRoadService;
            _mapper = mapper;
        }
       
        public async Task<Response<ICollection<RoadCorridorDto>>> Handle(GetRoadStatusQuery request, CancellationToken cancellationToken)
        {

            var result = await _asyncRoadService.GetRoadCorridorAsync(request.GetRoadStatusParameter.RoadCode,
                appid: request.GetRoadStatusParameter.Appid,
                appkey: request.GetRoadStatusParameter.AppKey);

            var MappedResult = _mapper.Map<ICollection<RoadCorridorDto>>(result);

            return new Response<ICollection<RoadCorridorDto>> (MappedResult);
        }
    }
}
