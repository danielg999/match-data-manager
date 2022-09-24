using AutoMapper;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MatchDataManager.Api.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IMapper _mapper;
        private readonly MatchDataManagerDbContext _dbContext;

        public LocationRepository(IMapper mapper, MatchDataManagerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Guid> Create(LocationCreateDto dto)
        {
            var location = _mapper.Map<Location>(dto);

            location.Id = Guid.NewGuid();
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync();
            return location.Id;
        }

        public async Task<bool> IsAnyExistOnCreate(string name)
        {
            return await _dbContext.Locations.AnyAsync(l => l.Name == name);
        }

        public async Task<bool> IsAnyExistOnUpdate(string name, Guid id)
        {
            return await _dbContext.Locations.AnyAsync(l => l.Name == name && l.Id != id);
        }

        public async Task Delete(Guid id)
        {
            var location = _dbContext.Locations.FirstOrDefault(x => x.Id == id);

            if (location is null)
            {
                throw new NotFoundException("Location not found.");
            }

            _dbContext.Locations.Remove(location);
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<LocationDto>> GetAll()
        {
            var locations = await _dbContext.Locations.ToListAsync();
            var locationsDtos = _mapper.Map<List<LocationDto>>(locations);

            return locationsDtos;
        }

        public async Task<LocationDto> Get(Guid id)
        {
            var location = await _dbContext.Locations.FirstOrDefaultAsync(x => x.Id == id);
            var locationDto = _mapper.Map<LocationDto>(location);

            return locationDto;
        }

        public async Task Update(LocationUpdateDto dto)
        {
            var location = await _dbContext.Locations.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (dto is null)
            {
                throw new BadRequestException("Location doesn't exist.");
            }
            else if (location is null)
            {
                throw new NotFoundException("Location not found.");
            }

            location.City = dto.City;
            location.Name = dto.Name;

            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<int> CountAllLocations()
        {
            return await _dbContext.Locations.CountAsync();
        }
    }
}