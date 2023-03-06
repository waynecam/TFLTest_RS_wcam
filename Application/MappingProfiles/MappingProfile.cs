using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<RoadCorridor, RoadCorridorDto>()
                .ReverseMap();

            CreateMap<IRoadCorridor, RoadCorridorDto>();

        }

    }
}
