using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Services;

public class LocationService : ILocationService
{
    private readonly MatchDataManagerDbContext _dbContext;

    public LocationService(MatchDataManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddLocation(Location location)
    {
        location.Id = Guid.NewGuid();
        _dbContext.Locations.Add(location);
    }

    public void DeleteLocation(Guid locationId)
    {
        var location = _dbContext.Locations.FirstOrDefault(x => x.Id == locationId);
        if (location is not null)
        {
            _dbContext.Locations.Remove(location);
        }
    }

    public IEnumerable<Location> GetAllLocations()
    {
        return _dbContext.Locations;
    }

    public Location GetLocationById(Guid id)
    {
        return _dbContext.Locations.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateLocation(Location location)
    {
        var existingLocation = _dbContext.Locations.FirstOrDefault(x => x.Id == location.Id);
        if (existingLocation is null || location is null)
        {
            throw new ArgumentException("Location doesn't exist.", nameof(location));
        }

        existingLocation.City = location.City;
        existingLocation.Name = location.Name;
    }
}