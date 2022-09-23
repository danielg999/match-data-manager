using AutoMapper;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Services;

public class LocationService : ILocationService
{
    private readonly MatchDataManagerDbContext _dbContext;
    private readonly IMapper _mapper;

    public LocationService(MatchDataManagerDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Guid Create(LocationCreateDto dto)
    {
        var location = _mapper.Map<Location>(dto);

        location.Id = Guid.NewGuid();
        _dbContext.Locations.Add(location);
        _dbContext.SaveChanges();

        return location.Id;
    }

    public void Delete(Guid id)
    {
        var location = _dbContext.Locations.FirstOrDefault(x => x.Id == id);

        if (location is null)
        {
            throw new NotFoundException("Location not found.");
        }

        _dbContext.Locations.Remove(location);
        _dbContext.SaveChanges();
    }

    public IEnumerable<LocationDto> GetAll()
    {
        var locations = _dbContext.Locations.ToList();
        var locationsDtos = _mapper.Map<List<LocationDto>>(locations);

        return locationsDtos;
    }

    public LocationDto Get(Guid id)
    {
        var location = _dbContext.Locations.FirstOrDefault(x => x.Id == id);
        var locationDto = _mapper.Map<LocationDto>(location);

        return locationDto;
    }

    public void Update(Guid id, LocationUpdateDto dto)
    {
        var location = _dbContext.Locations.FirstOrDefault(x => x.Id == id);

        if (dto is null)
        {
            throw new BadRequestException("Location doesn't exist.");
        }
        else if (location is null)
        {
            throw new NotFoundException("Location not found.");
        }

        location.City = location.City;
        location.Name = location.Name;

        _dbContext.SaveChanges();
    }
}