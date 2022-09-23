using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MatchDataManager.Api
{
    public class MatchDataManagerMappingProfile : Profile
    {
        public MatchDataManagerMappingProfile()
        {
            CreateMap<LocationCreateDto, Location>();
            CreateMap<Location, LocationDto>();
        }
    }
}
