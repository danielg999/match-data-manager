using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Services;

public interface ILocationsService
{
    void AddLocation(Location location);
    void DeleteLocation(Guid locationId);
    IEnumerable<Location> GetAllLocations();
    Location GetLocationById(Guid id);
    void UpdateLocation(Location location);
}