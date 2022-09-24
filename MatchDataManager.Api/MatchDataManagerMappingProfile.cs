using AutoMapper;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Api
{
    public class MatchDataManagerMappingProfile : Profile
    {
        public MatchDataManagerMappingProfile()
        {
            CreateMap<LocationCreateDto, Location>();
            CreateMap<Location, LocationDto>();

            CreateMap<Team, TeamDto>();
            CreateMap<TeamCreateDto, Team>();
        }
    }
}