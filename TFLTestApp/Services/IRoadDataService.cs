using Application.Dtos;
using Application.Features.Roads.Queries.GetRoadStatus;
using Application.Responses;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFLTestApp.Services
{
    public interface IRoadDataService
    {
        Task<Response<ICollection<RoadCorridorDto>>> GetRoadCorridorAsync(GetRoadStatusParameter getRoadStatusParameter);
    }
}
