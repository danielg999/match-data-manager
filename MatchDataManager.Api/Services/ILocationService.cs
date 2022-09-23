using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Services;

public interface ILocationService
{
    Guid Create(LocationCreateDto dto);
    void Delete(Guid id);
    IEnumerable<LocationDto> GetAll();
    LocationDto Get(Guid id);
    void Update(Guid id, LocationUpdateDto dto);
}