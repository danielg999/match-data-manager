using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Services;

public interface ILocationService
{
    Task<Guid> Create(LocationCreateDto dto);

    Task Delete(Guid id);

    Task<IEnumerable<LocationDto>> GetAll();

    Task<LocationDto> Get(Guid id);

    Task Update(LocationUpdateDto dto);
}